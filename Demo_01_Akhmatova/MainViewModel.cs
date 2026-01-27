using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private Entities Context => Entities.GetContext();
        private ServiceRequest _selectedRequest;
        private Employee _selectedEmployeeFilter;
        private Apartment _selectedAddressFilter;
        private string _loadErrorMessage;

        public MainViewModel()
        {
            Requests = new ObservableCollection<ServiceRequest>();
            Addresses = new ObservableCollection<Apartment>();
            Employees = new ObservableCollection<Employee>();
            StatusOptions = new ObservableCollection<RequestStatus>();
            EmployeeFilters = new ObservableCollection<Employee>();
            AddressFilters = new ObservableCollection<Apartment>();
            FilteredRequestsByEmployee = new ObservableCollection<RequestHistoryItem>();
            FilteredRequestsByAddress = new ObservableCollection<RequestHistoryItem>();
            LoadData();
        }

        public ObservableCollection<ServiceRequest> Requests { get; }

        public ObservableCollection<Apartment> Addresses { get; }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<RequestStatus> StatusOptions { get; }

        public ObservableCollection<Employee> EmployeeFilters { get; }

        public ObservableCollection<Apartment> AddressFilters { get; }

        public ObservableCollection<RequestHistoryItem> FilteredRequestsByEmployee { get; }

        public ObservableCollection<RequestHistoryItem> FilteredRequestsByAddress { get; }

        public ServiceRequest SelectedRequest
        {
            get => _selectedRequest;
            set => SetProperty(ref _selectedRequest, value);
        }

        public Employee SelectedEmployeeFilter
        {
            get => _selectedEmployeeFilter;
            set
            {
                if (SetProperty(ref _selectedEmployeeFilter, value))
                {
                    RefreshHistoryByEmployee();
                }
            }
        }

        public Apartment SelectedAddressFilter
        {
            get => _selectedAddressFilter;
            set
            {
                if (SetProperty(ref _selectedAddressFilter, value))
                {
                    RefreshHistoryByAddress();
                }
            }
        }

        public string LoadErrorMessage
        {
            get => _loadErrorMessage;
            private set => SetProperty(ref _loadErrorMessage, value);
        }

        public bool HasLoadError => !string.IsNullOrWhiteSpace(LoadErrorMessage);

        public bool TryAddRequest(ServiceRequest request, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                request.ApplyToEntity();
                request.Entity.CreatedAt = DateTime.Now;
                request.Entity.UpdatedAt = DateTime.Now;
                var context = Entities.GetContext();
                context.Request.Add(request.Entity);
                AddHistoryEntry(context, request.Entity, "Создана новая заявка.");
                context.SaveChanges();
                request.UpdateFromEntity();
                Requests.Add(request);
                RefreshHistory();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Не удалось добавить заявку. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        public bool TryUpdateRequest(ServiceRequest request, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                request.ApplyToEntity();
                request.Entity.UpdatedAt = DateTime.Now;
                var context = Entities.GetContext();
                AddHistoryEntry(context, request.Entity, "Данные заявки обновлены.");
                context.SaveChanges();
                request.UpdateFromEntity();
                RefreshHistory();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Не удалось обновить заявку. Проверьте данные и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        public bool TryDeleteRequest(ServiceRequest request, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var histories = context.RequestHistory.Where(history => history.RequestId == request.Entity.RequestId).ToList();
                if (histories.Any())
                {
                    context.RequestHistory.RemoveRange(histories);
                }

                context.Request.Remove(request.Entity);
                context.SaveChanges();
                Requests.Remove(request);
                RefreshHistory();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Не удалось удалить заявку. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        public void RefreshHistory()
        {
            RefreshHistoryByEmployee();
            RefreshHistoryByAddress();
        }

        private void LoadData()
        {
            try
            {
                LoadErrorMessage = null;
                LoadAddresses();
                LoadEmployees();
                LoadStatuses();
                LoadRequests();
                RefreshFilters();
                RefreshHistory();
            }
            catch (Exception ex)
            {
                LoadErrorMessage = $"Не удалось загрузить данные из базы. Проверьте подключение.\n{ex.Message}";
            }
        }

        private void LoadAddresses()
        {
            Addresses.Clear();
            var addresses = Context.Apartment
                .AsNoTracking()
                .Include(apartment => apartment.Building)
                .OrderBy(apartment => apartment.Building.Address)
                .ThenBy(apartment => apartment.ApartmentNumber)
                .ToList();

            foreach (var address in addresses)
            {
                Addresses.Add(address);
            }
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            var employees = Context.Employee
                .AsNoTracking()
                .OrderBy(employee => employee.FullName)
                .ToList();

            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private void LoadStatuses()
        {
            StatusOptions.Clear();
            var statuses = Context.RequestStatus
                .AsNoTracking()
                .OrderBy(status => status.StatusName)
                .ToList();

            foreach (var status in statuses)
            {
                StatusOptions.Add(status);
            }
        }

        private void LoadRequests()
        {
            Requests.Clear();
            var requests = Context.Request
                .AsNoTracking()
                .Include(request => request.Apartment.Building)
                .Include(request => request.Employee)
                .Include(request => request.RequestStatus)
                .OrderByDescending(request => request.CreatedAt)
                .ToList();

            foreach (var request in requests)
            {
                Requests.Add(new ServiceRequest(request));
            }
        }

        private void RefreshFilters()
        {
            EmployeeFilters.Clear();
            foreach (var employee in Employees)
            {
                EmployeeFilters.Add(employee);
            }

            AddressFilters.Clear();
            foreach (var address in Addresses)
            {
                AddressFilters.Add(address);
            }

            if (SelectedEmployeeFilter == null && EmployeeFilters.Any())
            {
                SelectedEmployeeFilter = EmployeeFilters.First();
            }

            if (SelectedAddressFilter == null && AddressFilters.Any())
            {
                SelectedAddressFilter = AddressFilters.First();
            }
        }

        private void RefreshHistoryByEmployee()
        {
            FilteredRequestsByEmployee.Clear();

            if (SelectedEmployeeFilter == null)
            {
                return;
            }

            var historyEntries = Context.RequestHistory
                .AsNoTracking()
                .Include(history => history.Request.Apartment.Building)
                .Include(history => history.Request)
                .Include(history => history.RequestStatus)
                .Include(history => history.Employee)
                .Where(history => history.EmployeeId == SelectedEmployeeFilter.EmployeeId)
                .OrderByDescending(history => history.ChangedAt)
                .ToList();

            foreach (var entry in historyEntries)
            {
                FilteredRequestsByEmployee.Add(MapHistoryItem(entry));
            }
        }

        private void RefreshHistoryByAddress()
        {
            FilteredRequestsByAddress.Clear();

            if (SelectedAddressFilter == null)
            {
                return;
            }

            var historyEntries = Context.RequestHistory
                .AsNoTracking()
                .Include(history => history.Request.Apartment.Building)
                .Include(history => history.Request)
                .Include(history => history.RequestStatus)
                .Include(history => history.Employee)
                .Where(history => history.Request.ApartmentId == SelectedAddressFilter.ApartmentId)
                .OrderByDescending(history => history.ChangedAt)
                .ToList();

            foreach (var entry in historyEntries)
            {
                FilteredRequestsByAddress.Add(MapHistoryItem(entry));
            }
        }

        private static void AddHistoryEntry(Entities context, Request request, string comment)
        {
            var history = new RequestHistory
            {
                Request = request,
                ChangedAt = DateTime.Now,
                StatusId = request.StatusId,
                EmployeeId = request.AssignedEmployeeId,
                Comment = comment
            };

            context.RequestHistory.Add(history);
        }

        private static RequestHistoryItem MapHistoryItem(RequestHistory history)
        {
            var request = history.Request;
            return new RequestHistoryItem
            {
                Id = history.RequestId,
                AddressDisplay = request?.Apartment?.AddressLine ?? "Адрес не указан",
                ApplicantName = request?.ApplicantFullName ?? "Не указан",
                Description = request?.ProblemDescription ?? string.Empty,
                Status = history.RequestStatus?.StatusName ?? request?.RequestStatus?.StatusName ?? "Не указан",
                EmployeeDisplay = history.Employee?.FullName ?? request?.Employee?.FullName ?? "Не назначен"
            };
        }
    }
}