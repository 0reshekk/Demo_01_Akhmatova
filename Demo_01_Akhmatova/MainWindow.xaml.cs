using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Demo_01_Akhmatova.Models;
using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            if (_viewModel.HasLoadError)
            {
                MessageBox.Show(
                    _viewModel.LoadErrorMessage,
                    "Ошибка загрузки данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddRequestClick(object sender, RoutedEventArgs e)
        {
            var newRequest = new ServiceRequest();
            var editorViewModel = new RequestEditorViewModel(newRequest, _viewModel.Addresses, _viewModel.Employees, _viewModel.StatusOptions, true);
            var editorWindow = new RequestWindow(editorViewModel)
            {
                Owner = this
            };

            if (editorWindow.ShowDialog() == true)
            {
                if (!_viewModel.TryAddRequest(newRequest, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditRequestClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null)
            {
                MessageBox.Show(
                    "Выберите заявку для редактирования.",
                    "Редактирование заявки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var editorViewModel = new RequestEditorViewModel(_viewModel.SelectedRequest, _viewModel.Addresses, _viewModel.Employees, _viewModel.StatusOptions, false);
            var editorWindow = new RequestWindow(editorViewModel)
            {
                Owner = this
            };

            if (editorWindow.ShowDialog() == true)
            {
                if (!_viewModel.TryUpdateRequest(_viewModel.SelectedRequest, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void DeleteRequestClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null)
            {
                MessageBox.Show(
                    "Выберите заявку для удаления.",
                    "Удаление заявки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранную заявку? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (!_viewModel.TryDeleteRequest(_viewModel.SelectedRequest, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка удаления",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    _viewModel.SelectedRequest = null;
                }
            }
        }

        private void RequestListClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRequest == null)
            {
                return;
            }

            if (sender is ListView)
            {
                var source = e.OriginalSource as DependencyObject;
                while (!(source == null || source is ListViewItem))
                {
                    source = VisualTreeHelper.GetParent(source);
                }

                if (source is ListViewItem)
                    EditRequestClick(sender, e);
            }
        }

        private void AddResidentClick(object sender, RoutedEventArgs e)
        {
            var resident = new Resident();
            var editorViewModel = new ResidentEditorViewModel(resident, true);
            var window = new ResidentWindow(editorViewModel)
            {
                Owner = this,
                Tag = resident
            };

            if (window.ShowDialog() == true)
            {
                if (!TryAddResident(resident, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditResidentClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedResident == null)
            {
                MessageBox.Show(
                    "Выберите жильца для редактирования.",
                    "Редактирование жильца",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var resident = _viewModel.SelectedResident;
            var editorViewModel = new ResidentEditorViewModel(resident, false);
            var window = new ResidentWindow(editorViewModel)
            {
                Owner = this,
                Tag = resident
            };

            if (window.ShowDialog() == true)
            {
                if (!TryUpdateResident(resident, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    // перезагружаем список, чтобы обновить привязки
                    _viewModel.SelectedResident = null;
                }
            }
        }

        private void DeleteResidentClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedResident == null)
            {
                MessageBox.Show(
                    "Выберите жильца для удаления.",
                    "Удаление жильца",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранного жильца? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (!TryDeleteResident(_viewModel.SelectedResident, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _viewModel.SelectedResident = null;
            }
        }

        private void AddEmployeeClick(object sender, RoutedEventArgs e)
        {
            var employee = new Employee { IsActive = true };
            var editorViewModel = new EmployeeEditorViewModel(employee, true);
            var window = new EmployeeWindow(editorViewModel)
            {
                Owner = this,
                Tag = employee
            };

            if (window.ShowDialog() == true)
            {
                if (!TryAddEmployee(employee, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditEmployeeClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedEmployee == null)
            {
                MessageBox.Show(
                    "Выберите сотрудника для редактирования.",
                    "Редактирование сотрудника",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var employee = _viewModel.SelectedEmployee;
            var editorViewModel = new EmployeeEditorViewModel(employee, false);
            var window = new EmployeeWindow(editorViewModel)
            {
                Owner = this,
                Tag = employee
            };

            if (window.ShowDialog() == true)
            {
                if (!TryUpdateEmployee(employee, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    _viewModel.SelectedEmployee = null;
                }
            }
        }

        private void DeleteEmployeeClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedEmployee == null)
            {
                MessageBox.Show(
                    "Выберите сотрудника для удаления.",
                    "Удаление сотрудника",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранного сотрудника? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (!TryDeleteEmployee(_viewModel.SelectedEmployee, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _viewModel.SelectedEmployee = null;
            }
        }

        private bool TryAddEmployee(Employee employee, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                context.Employee.Add(employee);
                context.SaveChanges();
                _viewModel.Employees.Add(employee);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось добавить сотрудника. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryUpdateEmployee(Employee employee, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Employee.Find(employee.EmployeeId);
                if (existing != null)
                {
                    existing.FullName = employee.FullName;
                    existing.Phone = employee.Phone;
                    existing.Position = employee.Position;
                    existing.IsActive = employee.IsActive;
                    context.SaveChanges();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось обновить данные сотрудника. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryDeleteEmployee(Employee employee, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Employee.Find(employee.EmployeeId);
                if (existing != null)
                {
                    context.Employee.Remove(existing);
                    context.SaveChanges();
                }

                _viewModel.Employees.Remove(employee);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось удалить сотрудника. Возможно, у него есть связанные заявки.\n{ex.Message}";
                return false;
            }
        }

        private void AddApartmentClick(object sender, RoutedEventArgs e)
        {
            var apartment = new Apartment();
            var editorViewModel = new ApartmentEditorViewModel(apartment, _viewModel.Buildings, true);
            var window = new ApartmentWindow(editorViewModel)
            {
                Owner = this,
                Tag = apartment
            };

            if (window.ShowDialog() == true)
            {
                if (!TryAddApartment(apartment, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditApartmentClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedAddress == null)
            {
                MessageBox.Show(
                    "Выберите адрес для редактирования.",
                    "Редактирование адреса",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var apartment = _viewModel.SelectedAddress;
            var editorViewModel = new ApartmentEditorViewModel(apartment, _viewModel.Buildings, false);
            var window = new ApartmentWindow(editorViewModel)
            {
                Owner = this,
                Tag = apartment
            };

            if (window.ShowDialog() == true)
            {
                if (!TryUpdateApartment(apartment, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    _viewModel.SelectedAddress = null;
                }
            }
        }

        private void DeleteApartmentClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedAddress == null)
            {
                MessageBox.Show(
                    "Выберите адрес для удаления.",
                    "Удаление адреса",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранный адрес? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (!TryDeleteApartment(_viewModel.SelectedAddress, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _viewModel.SelectedAddress = null;
            }
        }

        private bool TryAddApartment(Apartment apartment, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                context.Apartment.Add(apartment);
                context.SaveChanges();
                _viewModel.Addresses.Add(apartment);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось добавить адрес. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryUpdateApartment(Apartment apartment, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Apartment.Find(apartment.ApartmentId);
                if (existing != null)
                {
                    existing.BuildingId = apartment.BuildingId;
                    existing.ApartmentNumber = apartment.ApartmentNumber;
                    context.SaveChanges();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось обновить адрес. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryDeleteApartment(Apartment apartment, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Apartment.Find(apartment.ApartmentId);
                if (existing != null)
                {
                    context.Apartment.Remove(existing);
                    context.SaveChanges();
                }

                _viewModel.Addresses.Remove(apartment);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось удалить адрес. Возможно, с ним связаны другие записи.\n{ex.Message}";
                return false;
            }
        }

        private void AddBuildingClick(object sender, RoutedEventArgs e)
        {
            var building = new Building();
            var editorViewModel = new BuildingEditorViewModel(building, true);
            var window = new BuildingWindow(editorViewModel)
            {
                Owner = this,
                Tag = building
            };

            if (window.ShowDialog() == true)
            {
                if (!TryAddBuilding(building, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditBuildingClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedBuilding == null)
            {
                MessageBox.Show(
                    "Выберите дом для редактирования.",
                    "Редактирование дома",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var building = _viewModel.SelectedBuilding;
            var editorViewModel = new BuildingEditorViewModel(building, false);
            var window = new BuildingWindow(editorViewModel)
            {
                Owner = this,
                Tag = building
            };

            if (window.ShowDialog() == true)
            {
                if (!TryUpdateBuilding(building, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    _viewModel.SelectedBuilding = null;
                }
            }
        }

        private void DeleteBuildingClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedBuilding == null)
            {
                MessageBox.Show(
                    "Выберите дом для удаления.",
                    "Удаление дома",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранный дом? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (!TryDeleteBuilding(_viewModel.SelectedBuilding, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _viewModel.SelectedBuilding = null;
            }
        }

        private bool TryAddBuilding(Building building, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                context.Building.Add(building);
                context.SaveChanges();
                _viewModel.Buildings.Add(building);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось добавить дом. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryUpdateBuilding(Building building, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Building.Find(building.BuildingId);
                if (existing != null)
                {
                    existing.Address = building.Address;
                    existing.ManagementStartDate = building.ManagementStartDate;
                    existing.Floors = building.Floors;
                    existing.ApartmentsPlanned = building.ApartmentsPlanned;
                    existing.BuildYear = building.BuildYear;
                    existing.AreaM2 = building.AreaM2;
                    context.SaveChanges();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось обновить данные дома. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryDeleteBuilding(Building building, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Building.Find(building.BuildingId);
                if (existing != null)
                {
                    context.Building.Remove(existing);
                    context.SaveChanges();
                }

                _viewModel.Buildings.Remove(building);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось удалить дом. Возможно, с ним связаны квартиры или другие записи.\n{ex.Message}";
                return false;
            }
        }

        private void AddDebtClick(object sender, RoutedEventArgs e)
        {
            var debt = new Debt { AsOfDate = System.DateTime.Today };
            var editorViewModel = new DebtEditorViewModel(debt, _viewModel.Residents, _viewModel.Addresses, true);
            var window = new DebtWindow(editorViewModel)
            {
                Owner = this,
                Tag = debt
            };

            if (window.ShowDialog() == true)
            {
                if (!TryAddDebt(debt, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void EditDebtClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedDebt == null)
            {
                MessageBox.Show(
                    "Выберите запись задолженности для редактирования.",
                    "Редактирование задолженности",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var debt = _viewModel.SelectedDebt;
            var editorViewModel = new DebtEditorViewModel(debt, _viewModel.Residents, _viewModel.Addresses, false);
            var window = new DebtWindow(editorViewModel)
            {
                Owner = this,
                Tag = debt
            };

            if (window.ShowDialog() == true)
            {
                if (!TryUpdateDebt(debt, out var errorMessage))
                {
                    MessageBox.Show(
                        errorMessage,
                        "Ошибка сохранения",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                else
                {
                    _viewModel.SelectedDebt = null;
                    RefreshDebtView();
                }
            }
        }

        private void DeleteDebtClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedDebt == null)
            {
                MessageBox.Show(
                    "Выберите запись задолженности для удаления.",
                    "Удаление задолженности",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Удалить выбранную запись задолженности? Это действие невозможно отменить.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (!TryDeleteDebt(_viewModel.SelectedDebt, out var errorMessage))
            {
                MessageBox.Show(
                    errorMessage,
                    "Ошибка удаления",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                _viewModel.SelectedDebt = null;
                RefreshDebtView();
            }
        }

        private bool TryAddDebt(Debt debt, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                context.Debt.Add(debt);
                context.SaveChanges();
                _viewModel.Debts.Add(debt);
                RefreshDebtView();
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось добавить задолженность. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryUpdateDebt(Debt debt, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Debt.Find(debt.DebtId);
                if (existing != null)
                {
                    existing.ResidentId = debt.ResidentId;
                    existing.ApartmentId = debt.ApartmentId;
                    existing.AsOfDate = debt.AsOfDate;
                    existing.DebtWater = debt.DebtWater;
                    existing.DebtElectricity = debt.DebtElectricity;
                    context.SaveChanges();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось обновить задолженность. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryDeleteDebt(Debt debt, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Debt.Find(debt.DebtId);
                if (existing != null)
                {
                    context.Debt.Remove(existing);
                    context.SaveChanges();
                }

                _viewModel.Debts.Remove(debt);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось удалить задолженность.\n{ex.Message}";
                return false;
            }
        }

        private void RefreshDebtView()
        {
            // принудительно обновляем отфильтрованный список
            var currentDate = _viewModel.SelectedDebtDate;
            _viewModel.SelectedDebtDate = null;
            _viewModel.SelectedDebtDate = currentDate;
        }

        private bool TryAddResident(Resident resident, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                context.Resident.Add(resident);
                context.SaveChanges();
                _viewModel.Residents.Add(resident);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось добавить жильца. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryUpdateResident(Resident resident, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Resident.Find(resident.ResidentId);
                if (existing != null)
                {
                    existing.FullName = resident.FullName;
                    existing.Phone = resident.Phone;
                    context.SaveChanges();
                }

                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось обновить данные жильца. Проверьте соединение с базой данных и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private bool TryDeleteResident(Resident resident, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                var context = Entities.GetContext();
                var existing = context.Resident.Find(resident.ResidentId);
                if (existing != null)
                {
                    context.Resident.Remove(existing);
                    context.SaveChanges();
                }

                _viewModel.Residents.Remove(resident);
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Не удалось удалить жильца. Проверьте, нет ли у него связанных записей, и повторите попытку.\n{ex.Message}";
                return false;
            }
        }

        private void ClearDebtDateFilterClick(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedDebtDate = null;
        }
    }
}