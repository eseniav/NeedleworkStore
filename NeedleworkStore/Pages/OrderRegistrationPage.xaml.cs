using NeedleworkStore.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        List<PickUpPoints> pickUpPoints;
        Cities selectedCity;
        PickUpPoints selectedPickUpPoint;
        public OrderRegistrationPage(List<Carts> crt)
        {
            InitializeComponent();
            orderCart = crt;
            city = App.ctx.Cities.ToList();
            pickUpPoints = App.ctx.PickUpPoints.ToList();
            cmbPickUpPointCity.ItemsSource = city;
            cmbPickUpPointAddress.ItemsSource = pickUpPoints;
            selectedCity = null;
            selectedPickUpPoint = null;
            ICOrderReg.ItemsSource = orderCart;
            ICOrderReg.DataContext = orderCart;
            ChangeSelectedQuantityBottomMenu();
            SetTotalSum();
        }
        private void ChangeSelectedQuantityBottomMenu()
        {
            lblTotalQuantity.Content = orderCart.Sum(p => p.QuantityCart).ToString();
        }
        private void SetTotalSum() => lblTotalSum.Content = orderCart.Sum(c => c.SumProducts).ToString();
        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }
        private void MinusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity--;
            try
            {
                App.ctx.SaveChanges();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            MinusQuantity(selectedCartProd, (RepeatButton)sender);
        }
        private void PlusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity++;
            try
            {
                App.ctx.SaveChanges();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            PlusQuantity(selectedCartProd, (RepeatButton)sender);
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет конкретный товар.\nПеред удалением появляется специальное окошко с выбором");
        }

        private void btnPOrderReg_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Оформление заказа");
        }
        private void SetPickUpPointAddress(string address)
        {
            selectedPickUpPoint = App.ctx.PickUpPoints.FirstOrDefault(p => p.Adress == address);
            if (selectedCity == null)
            {
                selectedCity = App.ctx.Cities.FirstOrDefault(c => c.CityID == selectedPickUpPoint.CityID);
                cmbPickUpPointCity.SelectedItem = selectedCity;
            }
        }
        private void cmbPickUpPointAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointAddress(((PickUpPoints)((ComboBox)sender).SelectedItem).Adress);
        }
        private void FilterAddressList()
        {
            pickUpPoints = App.ctx.PickUpPoints.ToList();
            cmbPickUpPointAddress.SelectedItem = App.ctx.PickUpPoints.FirstOrDefault(c => c.CityID == selectedCity.CityID);
            pickUpPoints = App.ctx.PickUpPoints.Where(c => c.CityID == selectedCity.CityID).ToList();
            cmbPickUpPointAddress.ItemsSource = pickUpPoints;
        }
        private void cmbPayment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Выбор оплаты");
        }
        private void SetPickUpPointCity(string cityName)
        {
            selectedCity = App.ctx.Cities.FirstOrDefault(c => c.CityName == cityName);
            FilterAddressList();
        }
        private void cmbPickUpPointCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointCity(((Cities)((ComboBox)sender).SelectedItem).CityName);
        }
    }
}
