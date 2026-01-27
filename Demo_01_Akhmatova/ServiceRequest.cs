using System;

namespace Demo_01_Akhmatova.Models
{
    public class ServiceRequest : ObservableObject
    {
        private Apartment _address;
        private Employee _employee;
        private RequestStatus _status;
        private string _applicantName;
        private string _phone;
        private string _description;

        public ServiceRequest()
            : this(new Request
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            })
        {
        }

        public ServiceRequest(Request entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            UpdateFromEntity();
        }

        public Request Entity { get; }

        public int Id => Entity.RequestId;

        public Apartment Address
        {
            get => _address;
            set
            {
                if (SetProperty(ref _address, value))
                {
                    RaisePropertyChanged(nameof(AddressDisplay));
                }
            }
        }

        public Employee Employee
        {
            get => _employee;
            set
            {
                if (SetProperty(ref _employee, value))
                {
                    RaisePropertyChanged(nameof(EmployeeDisplay));
                }
            }
        }

        public RequestStatus StatusEntity
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
                {
                    RaisePropertyChanged(nameof(Status));
                }
            }
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

        public string AddressDisplay => Address?.AddressLine ?? "Адрес не указан";

        public string EmployeeDisplay => Employee?.FullName ?? "Не назначен";

        public string Status => StatusEntity?.StatusName ?? "Не указан";

        public void UpdateFromEntity()
        {
            Address = Entity.Apartment;
            Employee = Entity.Employee;
            StatusEntity = Entity.RequestStatus;
            ApplicantName = Entity.ApplicantFullName;
            Phone = Entity.ApplicantPhone;
            Description = Entity.ProblemDescription;
            RaisePropertyChanged(nameof(Id));
        }

        public void ApplyToEntity()
        {
            Entity.Apartment = Address;
            Entity.Employee = Employee;
            Entity.RequestStatus = StatusEntity;
            Entity.ApartmentId = Address?.ApartmentId ?? 0;
            Entity.AssignedEmployeeId = Employee?.EmployeeId;
            Entity.StatusId = StatusEntity?.StatusId ?? Entity.StatusId;
            Entity.ApplicantFullName = ApplicantName;
            Entity.ApplicantPhone = Phone;
            Entity.ProblemDescription = Description;
        }
    }
}