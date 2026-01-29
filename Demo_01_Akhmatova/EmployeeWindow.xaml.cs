using System.Windows;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow(EmployeeEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeEditorViewModel viewModel && Tag is Employee employee)
            {
                var validationMessage = viewModel.Validate();
                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    MessageBox.Show(
                        validationMessage,
                        "Ошибка ввода",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                viewModel.ApplyChanges(employee);
                DialogResult = true;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

