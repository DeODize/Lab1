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
        private ObservableCollection<StudentWork> works = new();

        public MainWindow()
        {
            InitializeComponent();
            WorksDataGrid.ItemsSource = works;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.CheckFileExists = false;
            var result = dlg.ShowDialog(this);
            if (result == true)
            {
                FilePathTextBox.Text = dlg.FileName;
            }
            var path = FilePathTextBox.Text.Trim();
            try
            {
                works.Clear();
                if (!System.IO.File.Exists(path))
                {
                    ResultLabel.Content = "Файл не найден. Будет создан при добавлении строки.";
                    return;
                }

                foreach (var line in System.IO.File.ReadLines(path, Encoding.UTF8))
                {
                    if (TryParseStudentWork(line, out var sw))
                        works.Add(sw);
                }

                ResultLabel.Content = "Файл загружен.";
            }
            catch (System.Exception ex)
            {
                ResultLabel.Content = "Ошибка: " + ex.Message;
            }
        }

        private bool TryParseStudentWork(string raw, out StudentWork work)
        {
            work = null;
            if (string.IsNullOrWhiteSpace(raw)) return false;

            try
            {
                var parts = raw.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim())
                               .Where(p => !string.IsNullOrEmpty(p))
                               .ToArray();

                if (parts.Length >= 3)
                {
                    var name = parts[0];
                    var nameOfWork = parts[1];
                    var dateStr = parts[2];
                    if (!System.DateTime.TryParse(dateStr, out var dt)) dt = System.DateTime.MinValue;
                    work = new StudentWork(name, nameOfWork, dt);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (works.Count == 0)
            {
                ResultLabel.Content = "Сначала загрузите файл.";
                return;
            }

            var dlg = new AddWorkWindow();
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                works.Add(dlg.Work);
                ResultLabel.Content = "Элемент добавлен (не сохранено).";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var path = FilePathTextBox.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                ResultLabel.Content = "Укажите путь к файлу.";
                return;
            }

            try
            {
                using var sw = new System.IO.StreamWriter(path, append: false, encoding: Encoding.UTF8);
                foreach (var w in works)
                {
                    sw.WriteLine(w.ToRawString());
                }

                ResultLabel.Content = "Сохранено в файл.";
            }
            catch (System.Exception ex)
            {
                ResultLabel.Content = "Ошибка: " + ex.Message;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorksDataGrid.SelectedItem is StudentWork sel)
            {
                works.Remove(sel);
                ResultLabel.Content = "Элемент удалён (не сохранено).";
            }
            else
            {
                ResultLabel.Content = "Выберите элемент для удаления.";
            }
        }
    }
}