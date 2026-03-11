using System;
using System.Windows;

namespace Lab2
{
    public partial class AddWorkWindow : Window
    {
        public StudentWork? Result { get; private set; }
        public AddWorkWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text?.Trim() ?? string.Empty;
            var work = WorkBox.Text?.Trim() ?? string.Empty;
            DateTime date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(DateBox.Text))
            {
                if (!DateTime.TryParse(DateBox.Text.Trim(), out date))
                {
                    MessageBox.Show(this, "Неверный формат даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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