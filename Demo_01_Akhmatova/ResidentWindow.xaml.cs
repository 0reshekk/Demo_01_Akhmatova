using System.Windows;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova
{
    public partial class ResidentWindow : Window
    {
        public ResidentWindow(ResidentEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ResidentEditorViewModel viewModel && Tag is Resident resident)
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

                viewModel.ApplyChanges(resident);
                DialogResult = true;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

