using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova.Models
{
    public class DebtEditorViewModel : ObservableObject
    {
        private readonly bool _isNew;
        private Resident _selectedResident;
        private Apartment _selectedApartment;
        private DateTime _asOfDate;
        private decimal? _debtWater;
        private decimal? _debtElectricity;

        public DebtEditorViewModel(
            Debt debt,
            IEnumerable<Resident> residents,
            IEnumerable<Apartment> apartments,
            bool isNew)
        {
            _isNew = isNew;
            Residents = new ObservableCollection<Resident>(residents ?? Enumerable.Empty<Resident>());
            Apartments = new ObservableCollection<Apartment>(apartments ?? Enumerable.Empty<Apartment>());

            _selectedResident = debt?.Resident ?? Residents.FirstOrDefault();
            _selectedApartment = debt?.Apartment ?? Apartments.FirstOrDefault();
            _asOfDate = debt?.AsOfDate ?? DateTime.Today;
            _debtWater = debt?.DebtWater;
            _debtElectricity = debt?.DebtElectricity;
        }

        public string WindowTitle => _isNew ? "Добавление задолженности" : "Редактирование задолженности";

        public ObservableCollection<Resident> Residents { get; }

        public ObservableCollection<Apartment> Apartments { get; }

        public Resident SelectedResident
        {
            get => _selectedResident;
            set => SetProperty(ref _selectedResident, value);
        }

        public Apartment SelectedApartment
        {
            get => _selectedApartment;
            set => SetProperty(ref _selectedApartment, value);
        }

        public DateTime AsOfDate
        {
            get => _asOfDate;
            set => SetProperty(ref _asOfDate, value);
        }

        public decimal? DebtWater
        {
            get => _debtWater;
            set => SetProperty(ref _debtWater, value);
        }

        public decimal? DebtElectricity
        {
            get => _debtElectricity;
            set => SetProperty(ref _debtElectricity, value);
        }

        public string Validate()
        {
            if (SelectedResident == null)
            {
                return "Выберите жильца для задолженности.";
            }

            if (SelectedApartment == null)
            {
                return "Выберите квартиру для задолженности.";
            }

            return string.Empty;
        }

        public void ApplyChanges(Debt target)
        {
            if (target == null)
            {
                return;
            }

            target.Resident = SelectedResident;
            target.ResidentId = SelectedResident?.ResidentId ?? target.ResidentId;
            target.Apartment = SelectedApartment;
            target.ApartmentId = SelectedApartment?.ApartmentId ?? target.ApartmentId;
            target.AsOfDate = AsOfDate;
            target.DebtWater = DebtWater;
            target.DebtElectricity = DebtElectricity;
        }
    }
}

