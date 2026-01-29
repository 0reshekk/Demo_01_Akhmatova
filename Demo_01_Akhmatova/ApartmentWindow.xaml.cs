using System.Windows;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova
{
    public partial class ApartmentWindow : Window
    {
        public ApartmentWindow(ApartmentEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is ApartmentEditorViewModel viewModel && Tag is Apartment apartment)
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

                viewModel.ApplyChanges(apartment);
                DialogResult = true;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

