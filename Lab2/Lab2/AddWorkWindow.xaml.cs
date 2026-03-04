using System;
using System.Windows;

namespace Lab2
{
    public partial class AddWorkWindow : Window
    {
        public StudentWork Work { get; private set; }

        public AddWorkWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text.Trim();
            var workName = WorkTextBox.Text.Trim();
            var dateText = DateTextBox.Text.Trim();
            DateTime dt = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateText))
            {
                DateTime.TryParse(dateText, out dt);
            }

            Work = new StudentWork(name, workName, dt);
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