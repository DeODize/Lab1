using System.Collections.ObjectModel;
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
            Logger.Init("log.txt", WriteLog);
            //Filter filter = new Filter(_works);
            //filter.OnGrid += Filter_OnGrid;
            //filter.Show();
        }

        private void Filter_OnGrid(ObservableCollection<StudentWork> filteredWorks)
        {
            Dispatcher.Invoke(() =>
            {
                WorksGrid.ItemsSource = filteredWorks;
            });
        }


        private void WriteLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogBox.AppendText(message + Environment.NewLine);
                LogBox.ScrollToEnd();
            });
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            _currentFilePath = FileHandler.OpenDialog();
            if (_currentFilePath != null)
            {
                Logger.Info($"Открыт файл: {_currentFilePath}");
                var parsedStrings = FileHandler.ReadFile(_currentFilePath);
                if (parsedStrings == null)
                {
                    Logger.Error("Ошибка чтения файла");
                    return;
                }

                var parsedObjects = ParseFile.FileParser(parsedStrings);
                if (parsedObjects != null)
                {
                    ParseFile.LoadObjects(parsedObjects, ref _works, ref _isFileLoaded);
                    Title = $"Работы студентов - {_currentFilePath}";
                    _perviousFilePath = _currentFilePath;
                }
                else
                {
                    Logger.Error("Ошибка чтения файла файла");
                    _currentFilePath = _perviousFilePath;
                }
            }
            else
            {
                Logger.Info("Файл не выбран");
            }
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
                Logger.Error("Ошибка записи файла");
            }
            else
            {
                Logger.Info($"Файл сохранен: {_currentFilePath}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddWorkWindow();
            win.Owner = this;
            if (win.ShowDialog() == true)
            {
                _works.Add(win.Result);
                Logger.Info("Добавлена работа: " + win.Result.ToRawString());
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorksGrid.SelectedItem is StudentWork work)
            {
                _works.Remove(work);
                Logger.Info("Удалена работа: " + work.ToRawString());
            }
            else
            {
                Logger.Info("Попытка удаления без выбора");
            }
        }
    }
}