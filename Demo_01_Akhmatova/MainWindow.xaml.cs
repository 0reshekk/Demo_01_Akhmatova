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
    }
}