using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova.Models
{
    public class ApartmentEditorViewModel : ObservableObject
    {
        private readonly bool _isNew;
        private Building _selectedBuilding;
        private int _apartmentNumber;

        public ApartmentEditorViewModel(
            Apartment apartment,
            IEnumerable<Building> buildings,
            bool isNew)
        {
            _isNew = isNew;
            Buildings = new ObservableCollection<Building>(buildings ?? Enumerable.Empty<Building>());
            _selectedBuilding = apartment?.Building ?? Buildings.FirstOrDefault();
            _apartmentNumber = apartment?.ApartmentNumber ?? 0;
        }

        public string WindowTitle => _isNew ? "Добавление адреса" : "Редактирование адреса";

        public ObservableCollection<Building> Buildings { get; }

        public Building SelectedBuilding
        {
            get => _selectedBuilding;
            set => SetProperty(ref _selectedBuilding, value);
        }

        public int ApartmentNumber
        {
            get => _apartmentNumber;
            set => SetProperty(ref _apartmentNumber, value);
        }

        public string Validate()
        {
            if (SelectedBuilding == null)
            {
                return "Выберите дом для квартиры.";
            }

            if (ApartmentNumber <= 0)
            {
                return "Укажите номер квартиры больше 0.";
            }

            return string.Empty;
        }

        public void ApplyChanges(Apartment target)
        {
            if (target == null)
            {
                return;
            }

            target.Building = SelectedBuilding;
            target.BuildingId = SelectedBuilding?.BuildingId ?? target.BuildingId;
            target.ApartmentNumber = ApartmentNumber;
        }
    }
}

