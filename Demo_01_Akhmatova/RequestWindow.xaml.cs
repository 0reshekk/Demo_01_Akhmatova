using System.Windows;
using Demo_01_Akhmatova.ViewModels;

namespace Demo_01_Akhmatova
{
    public partial class RequestWindow : Window
    {
        public RequestWindow(RequestEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is RequestEditorViewModel viewModel)
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

                viewModel.ApplyChanges();
                DialogResult = true;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}