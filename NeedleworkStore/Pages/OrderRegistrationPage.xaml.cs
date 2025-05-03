using NeedleworkStore.AppData;
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

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderRegistrationPage.xaml
    /// </summary>
    public partial class OrderRegistrationPage : Page
    {
        List<Carts> orderCart;
        List<Cities> city;
        public OrderRegistrationPage(List<Carts> crt)
        {
            InitializeComponent();
            orderCart = crt;
            city = App.ctx.Cities.ToList();
            cmbPickUpPointCity.ItemsSource = city;
            if (Int32.Parse(txblQuan.Text.ToString()) == 1)
            {
                btnMinus.IsEnabled = false;
            }
            if (Int32.Parse(txblQuan.Text.ToString()) == 99)
            {
                btnPlus.IsEnabled = false;
            }
        }
        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            int quant = Int32.Parse(txblQuan.Text.ToString());
            quant -= 1;
            txblQuan.Text = quant.ToString();
            if (Int32.Parse(txblQuan.Text.ToString()) == 1)
            {
                btnMinus.IsEnabled = false;
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(txblQuan.Text.ToString()) == 99)
            {
                MessageBox.Show("Достигнут лимит количества товаров");
                btnPlus.IsEnabled = false;
                return;
            }
            int quant = Int32.Parse(txblQuan.Text.ToString());
            quant += 1;
            txblQuan.Text = quant.ToString();
            if (Int32.Parse(txblQuan.Text.ToString()) > 1)
            {
                btnMinus.IsEnabled = true;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет конкретный товар.\nПеред удалением появляется специальное окошко с выбором");
        }

        private void btnPOrderReg_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Оформление заказа");
        }
        private void SetPickUpPointAddress()
        {
            MessageBox.Show("Выбор адреса пункта выдачи");
        }
        private void cmbPickUpPointAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointAddress();
        }

        private void cmbPayment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Выбор оплаты");
        }
        private void SetPickUpPointCity()
        {
            MessageBox.Show("Выбор города пункта выдачи");
        }
        private void cmbPickUpPointCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointCity();
        }
    }
}
