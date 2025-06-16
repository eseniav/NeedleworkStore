using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using NeedleworkStore.AppData;

namespace NeedleworkStore.UCElements
{
    /// <summary>
    /// Логика взаимодействия для Recomendations.xaml
    /// </summary>
    public partial class Recomendations : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Products> ListException
        {
            get => (List<Products>)GetValue(ListExceptionProperty);
            set => SetValue(ListExceptionProperty, value);
        }
        public static readonly DependencyProperty ListExceptionProperty =
            DependencyProperty.Register("ListException", typeof(List<Products>), typeof(Recomendations), new PropertyMetadata(new List<Products>()));

        private List<Products> products;
        private List<Products> productsList;
        public List<Products> ProductsList {
            get => productsList;
            set {
                productsList = value;
                OnPropertyChanged(nameof(ProductsList));
            }
        }
        public void RandomSubset()
        {
            Random random = new Random();
            ProductsList = products.OrderBy(p => random.Next()).Take(3).ToList();
        }
        public Recomendations()
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
                //ppCartIn.IsOpen = false;
                timer.Stop();
            };

            //ppCartIn.IsOpen = true;
            timer.Start();
        }
        private void btnFavor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавляет товар в избранное");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                //ppFavorIn.IsOpen = false;
                timer.Stop();
            };

            //ppFavorIn.IsOpen = true;
            timer.Start();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var excludedIds = ListException.Select(ex => ex.ProductID).ToList();
            products = App.ctx.Products.Where(p => !excludedIds.Contains(p.ProductID)).ToList();
            DataContext = this;
            RandomSubset();
        }
    }
}
