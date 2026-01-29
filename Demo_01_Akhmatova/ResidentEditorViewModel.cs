using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova.Models
{
    public class ResidentEditorViewModel : ObservableObject
    {
        private readonly bool _isNew;
        private string _fullName;
        private string _phone;

        public ResidentEditorViewModel(Resident resident, bool isNew)
        {
            _isNew = isNew;
            _fullName = resident?.FullName;
            _phone = resident?.Phone;
        }

        public string WindowTitle => _isNew ? "Добавление жильца" : "Редактирование жильца";

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

        public string Validate()
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                return "Укажите ФИО жильца.";
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                return "Укажите контактный телефон.";
            }

            return string.Empty;
        }

        public void ApplyChanges(Resident target)
        {
            if (target == null)
            {
                return;
            }

            target.FullName = FullName?.Trim();
            target.Phone = Phone?.Trim();
        }
    }
}

