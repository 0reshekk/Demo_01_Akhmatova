using System;
using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova.Models
{
    public class BuildingEditorViewModel : ObservableObject
    {
        private readonly bool _isNew;
        private string _address;
        private DateTime? _managementStartDate;
        private short? _floors;
        private short? _apartmentsPlanned;
        private short? _buildYear;
        private decimal? _areaM2;

        public BuildingEditorViewModel(Building building, bool isNew)
        {
            _isNew = isNew;
            _address = building?.Address;
            _managementStartDate = building?.ManagementStartDate;
            _floors = building?.Floors;
            _apartmentsPlanned = building?.ApartmentsPlanned;
            _buildYear = building?.BuildYear;
            _areaM2 = building?.AreaM2;
        }

        public string WindowTitle => _isNew ? "Добавление дома" : "Редактирование дома";

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public DateTime? ManagementStartDate
        {
            get => _managementStartDate;
            set => SetProperty(ref _managementStartDate, value);
        }

        public short? Floors
        {
            get => _floors;
            set => SetProperty(ref _floors, value);
        }

        public short? ApartmentsPlanned
        {
            get => _apartmentsPlanned;
            set => SetProperty(ref _apartmentsPlanned, value);
        }

        public short? BuildYear
        {
            get => _buildYear;
            set => SetProperty(ref _buildYear, value);
        }

        public decimal? AreaM2
        {
            get => _areaM2;
            set => SetProperty(ref _areaM2, value);
        }

        public string Validate()
        {
            if (string.IsNullOrWhiteSpace(Address))
            {
                return "Укажите адрес дома.";
            }

            return string.Empty;
        }

        public void ApplyChanges(Building target)
        {
            if (target == null)
            {
                return;
            }

            target.Address = Address?.Trim();
            target.ManagementStartDate = ManagementStartDate;
            target.Floors = Floors;
            target.ApartmentsPlanned = ApartmentsPlanned;
            target.BuildYear = BuildYear;
            target.AreaM2 = AreaM2;
        }
    }
}

