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

namespace Lab4
{
    public partial class Filter : Window
    {
        private ObservableCollection<StudentWork> WorksForFilter { get; }
        public Action<ObservableCollection<StudentWork>> OnGrid; // подписка UI
        public Filter(ObservableCollection<StudentWork> works)
        {
            InitializeComponent();
            WorksForFilter = works ?? new ObservableCollection<StudentWork>();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            // Получаем отфильтрованную коллекцию
            var filtered = ParseTextFile.Filter(WorksForFilter, FilterBox.Text);

            // Если нет подписчиков — ничего не делаем
            if (OnGrid == null)
                return;

            // Вызываем делегат в потоке UI через Dispatcher, если требуется
            if (Dispatcher.CheckAccess())
            {
                OnGrid(filtered);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => OnGrid(filtered)));
            }
        }
    }
}

