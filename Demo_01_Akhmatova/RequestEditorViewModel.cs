
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova.ViewModels
{
    public class RequestEditorViewModel : ObservableObject
    {
        private readonly ServiceRequest _request;
        private readonly bool _isNew;
        private Apartment _selectedAddress;
        private Employee _selectedEmployee;
        private RequestStatus _selectedStatus;
        private string _applicantName;
        private string _phone;
        private string _description;

        public RequestEditorViewModel(
            ServiceRequest request,
            IEnumerable<Apartment> addresses,
            IEnumerable<Employee> employees,
            IEnumerable<RequestStatus> statuses,
            bool isNew)
        {
            _request = request;
            _isNew = isNew;
            Addresses = new ObservableCollection<Apartment>(addresses ?? Enumerable.Empty<Apartment>());
            Employees = new ObservableCollection<Employee>(employees ?? Enumerable.Empty<Employee>());
            StatusOptions = new ObservableCollection<RequestStatus>(statuses ?? Enumerable.Empty<RequestStatus>());
            PopulateFromRequest();
        }

        public string WindowTitle => _isNew ? "Добавление заявки" : "Редактирование заявки";

        public ObservableCollection<Apartment> Addresses { get; }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<RequestStatus> StatusOptions { get; }

        public Apartment SelectedAddress
        {
            get => _selectedAddress;
            set => SetProperty(ref _selectedAddress, value);
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        public RequestStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public string ApplicantName
        {
            get => _applicantName;
            set => SetProperty(ref _applicantName, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Validate()
        {
            if (SelectedAddress == null)
            {
                return "Выберите адрес заявки.";
            }

            if (string.IsNullOrWhiteSpace(ApplicantName))
            {
                return "Укажите ФИО заявителя.";
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                return "Введите контактный телефон заявителя.";
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                return "Добавьте описание проблемы.";
            }

            if (SelectedEmployee == null)
            {
                return "Назначьте ответственного исполнителя заявки.";
            }

            if (SelectedStatus == null)
            {
                return "Укажите статус заявки.";
            }

            return string.Empty;
        }

        public void ApplyChanges()
        {
            _request.Address = SelectedAddress;
            _request.Employee = SelectedEmployee;
            _request.StatusEntity = SelectedStatus;
            _request.ApplicantName = ApplicantName?.Trim();
            _request.Phone = Phone?.Trim();
            _request.Description = Description?.Trim();
            _request.ApplyToEntity();
        }

        private void PopulateFromRequest()
        {
            _request.UpdateFromEntity();
            SelectedAddress = _request.Address ?? Addresses.FirstOrDefault();
            SelectedEmployee = _request.Employee ?? Employees.FirstOrDefault();
            SelectedStatus = _request.StatusEntity ?? StatusOptions.FirstOrDefault();
            ApplicantName = _request.ApplicantName;
            Phone = _request.Phone;
            Description = _request.Description;
        }
    }
}