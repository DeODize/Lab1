using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        private readonly System.Collections.ObjectModel.ObservableCollection<StudentWork> _works = new();
        private string _currentFilePath;
        private string _perviousFilePath;
        private bool _isFileLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            WorksGrid.ItemsSource = _works;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            _currentFilePath = ParseFile.OpenDialog();
            if (_currentFilePath != null)
            {
                var parsedObjects = ParseFile.ReadFile(_currentFilePath);
                if (parsedObjects != null)
                {
                    if (!_isFileLoaded)
                    {
                        var _tempWorks = new List<StudentWork>(_works);
                        _works.Clear();
                        foreach (var obj in parsedObjects)
                            _works.Add(obj);
                        foreach (var obj in _tempWorks)
                            _works.Add(obj);
                        _isFileLoaded = true;
                    }
                    else
                    {
                        _works.Clear();
                        foreach (var obj in parsedObjects)
                            _works.Add(obj);
                    }

                    Title = $"Работы студентов - {_currentFilePath}";
                    _perviousFilePath = _currentFilePath;
                }
                else
                {
                    MessageBox.Show(this, "Ошибка чтения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    _currentFilePath = _perviousFilePath;
                }
            }
            else MessageBox.Show(this, "Файл не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFilePath == null)
            {
                _currentFilePath = ParseFile.SaveDialog();
                Title = $"Работы студентов - {_currentFilePath}";
                _isFileLoaded = true;
            }
            if (!ParseFile.WriteFile(_currentFilePath, _works.ToList()))
            {
                MessageBox.Show(this, "Ошибка записи файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddWorkWindow();
            win.Owner = this;
            if (win.ShowDialog() == true)
            {
                _works.Add(win.Result);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _works.Remove((StudentWork)WorksGrid.SelectedItem);
        }
    }
}