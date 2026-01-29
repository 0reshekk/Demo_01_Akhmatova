using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova.Models
{
    public class EmployeeEditorViewModel : ObservableObject
    {
        private readonly bool _isNew;
        private string _fullName;
        private string _phone;
        private string _position;
        private bool _isActive;

        public EmployeeEditorViewModel(Employee employee, bool isNew)
        {
            _isNew = isNew;
            _fullName = employee?.FullName;
            _phone = employee?.Phone;
            _position = employee?.Position;
            _isActive = employee?.IsActive ?? true;
        }

        public string WindowTitle => _isNew ? "Добавление сотрудника" : "Редактирование сотрудника";

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string Validate()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                return "Укажите ФИО сотрудника.";
            }

            if (string.IsNullOrWhiteSpace(Position))
            {
                return "Укажите должность сотрудника.";
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                return "Укажите контактный телефон.";
            }

            return string.Empty;
        }

        public void ApplyChanges(Employee target)
        {
            if (target == null)
            {
                return;
            }

            target.FullName = FullName?.Trim();
            target.Phone = Phone?.Trim();
            target.Position = Position?.Trim();
            target.IsActive = IsActive;
        }
    }
}

