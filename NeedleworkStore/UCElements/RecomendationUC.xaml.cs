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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NeedleworkStore.UCElements
{
    /// <summary>
    /// Логика взаимодействия для RecomendationUC.xaml
    /// </summary>
    public partial class RecomendationUC : UserControl
    {
        public RecomendationUC()
        {
            InitializeComponent();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Переход на страницу с этим товаром");
        }
        private void btnCartIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в корзину");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                ppCartIn.IsOpen = false;
                timer.Stop();
            };

            ppCartIn.IsOpen = true;
            timer.Start();
        }
        private void btnFavor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в избранное");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                ppFavorIn.IsOpen = false;
                timer.Stop();
            };

            ppFavorIn.IsOpen = true;
            timer.Start();
        }
    }
}
