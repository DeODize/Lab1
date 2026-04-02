using System.Collections.ObjectModel;
using System.IO;
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

namespace Lab4
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<StudentWork> _works = new();
        private string _currentFilePath;
        private string _perviousFilePath;
        private bool _isFileLoaded = false;

        public MainWindow()
        {
            InitializeComponent();
            WorksGrid.ItemsSource = _works;
            Logger.Init("log.txt", WriteLog);
        }

        private void WriteLog(string message)
        {
            LogBox.AppendText(message + Environment.NewLine);
            LogBox.ScrollToEnd();
        }

        private void RemoveStudentWorks(List<StudentWork> StudWorks)
        {
            for (int i = _works.Count - 1; i >= 0; i--)
            {
                if (StudWorks.Contains(_works[i]))
                {
                    _works.RemoveAt(i);
                }
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            _currentFilePath = FileHandler.OpenDialog();
            if (_currentFilePath != null)
            {
                var parsedStrings = FileHandler.ReadFile(_currentFilePath);
                if (parsedStrings == null)
                {
                    Logger.Error("Ошибка чтения файла");
                    return;
                }

                var parsedObjects = ParseTextFile.FileParser(parsedStrings);
                if (parsedObjects != null)
                {
                    ParseTextFile.LoadObjects(parsedObjects, ref _works, ref _isFileLoaded);
                    Title = $"Работы студентов - {_currentFilePath}";
                    _perviousFilePath = _currentFilePath;
                }
                else
                {
                    Logger.Error("Ошибка чтения файла файла");
                    _currentFilePath = _perviousFilePath;
                }
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
            if (WorksGrid.SelectedItem is StudentWork work)
            {
                _works.Remove(work);
            }
            else
            {
                Logger.Error("Попытка удаления без выбора");
            }
        }

        private void CommandButton_Click(object sender, RoutedEventArgs e)
        {
            string commadsPath = FileHandler.OpenDialog();
            if (!System.IO.File.Exists(commadsPath))
            {
                Logger.Error($"Файл команд не найден: {commadsPath}");
                return;
            }
            var lines = FileHandler.ReadFile(commadsPath);

            if (lines == null) 
            {
                Logger.Error($"Ошибка чтения файла команд: {commadsPath}");
                return;
            }

            var parser = new ParseCommands
            {
                AddWorkAction = work => _works.Add(work),
                GetWorksFunc = () => _works,
                RemoveWorksAction = Studworks =>
                {
                    foreach (var w in Studworks)
                    {
                        _works.Remove(w);
                    } 
                },
                SaveWorks = spath  =>
                {
                    if (!FileHandler.WriteFile(spath, _works.ToList()))
                    {
                        Logger.Error("Ошибка сохранения файла из команды");
                    }
                }
            };
            parser.ProcessCommandFile(lines);
        }
    }
}