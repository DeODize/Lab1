using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab3
{
    /// <summary>
    /// Логика взаимодействия для AddWorkWindow.xaml
    /// </summary>
    public partial class AddWorkWindow : Window
    {
        public StudentWork? Result { get; private set; }
        public AddWorkWindow()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text?.Trim();
            var work = WorkBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(this, "Имя не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Logger.Error("Задано пустое имя");
                return;
            }
            if (string.IsNullOrWhiteSpace(work))
            {
                MessageBox.Show(this, "Название работы не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Logger.Error("Задано пустое название работы");
                return;
            }
            DateTime date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(DateBox.Text))
            {
                if (!DateTime.TryParse(DateBox.Text.Trim(), out date))
                {
                    MessageBox.Show(this, "Неверный формат даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Logger.Error("Неверный формат даты");
                    return;
                }
            }

            Result = new StudentWork(name, work, date);
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
