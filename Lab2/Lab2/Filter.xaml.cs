using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public Filter()
        {
            InitializeComponent();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).WorksGrid.SelectAll();
            List<StudentWork> studentWorks = new List<StudentWork>();

            foreach ( var item in ((MainWindow)Application.Current.MainWindow).WorksGrid.Items)
            {
                var studItem = item as StudentWork;
                studentWorks.Add(studItem);
            }
            ObservableCollection<StudentWork> studentFilteredWorks = ParseFile.Filter(studentWorks,FilterBox.Text);

            ((MainWindow)Application.Current.MainWindow).WorksGrid.ItemsSource = studentFilteredWorks;

            //foreach (var item in studentFilteredWorks)
            //{
            //    ((MainWindow)Application.Current.MainWindow).WorksGrid.Items.Add(item);
            //}
        }
    }
}
