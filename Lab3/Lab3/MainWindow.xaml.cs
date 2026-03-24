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

namespace Lab3
{
    public partial class MainWindow : Window
    {
        private System.Collections.ObjectModel.ObservableCollection<StudentWork> _works = new();
        private string _currentFilePath;
        private string _perviousFilePath;
        private bool _isFileLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            WorksGrid.ItemsSource = _works;
            //Filter filter = new Filter(_works);
            //filter.Show();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            _currentFilePath = FileHandler.OpenDialog();
            if (_currentFilePath != null)
            {
                var parsedStrings = FileHandler.ReadFile(_currentFilePath);
                var parsedObjects = ParseFile.FileParser(parsedStrings);
                if (parsedObjects != null) 
                {
                    ParseFile.LoadObjects(parsedObjects, ref _works, ref _isFileLoaded);
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
                _currentFilePath = FileHandler.SaveDialog();
                Title = $"Работы студентов - {_currentFilePath}";
                _isFileLoaded = true;
            }
            if (!FileHandler.WriteFile(_currentFilePath, _works.ToList()))
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