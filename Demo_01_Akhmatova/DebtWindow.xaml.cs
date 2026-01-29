using System.Windows;
using Demo_01_Akhmatova.Models;

namespace Demo_01_Akhmatova
{
    public partial class DebtWindow : Window
    {
        public DebtWindow(DebtEditorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is DebtEditorViewModel viewModel && Tag is Debt debt)
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

                viewModel.ApplyChanges(debt);
                DialogResult = true;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

