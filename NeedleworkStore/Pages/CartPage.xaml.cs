﻿using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;
using static NeedleworkStore.Pages.ProductsPage;

namespace NeedleworkStore.Pages{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        ObservableCollection<Carts> cart;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public int? TotalSum;
        DispatcherTimer saveTimer;
        public CartPage()
        {
            InitializeComponent();
            ResetCart();
            ICCart.DataContext = cart;
            ChangeFullEmptyCart();
            SetSelectedProdSumQuantity();
            cmbIAvail.IsSelected = true;
            mainWindow.UpdateCartState();
            saveTimer = new DispatcherTimer();
            saveTimer.Interval = TimeSpan.FromMilliseconds(500);
            saveTimer.Tick += OnSaveCart;
            mainWindow.btnProd.IsEnabled = true;
            RecomendUC.ListException = cart.Select(p => p.Products).ToList();
            RecomendUC.ProductAddedToCart += OnProductAddedToCart;
        }
        private void OnProductAddedToCart(Products product)
        {
            ResetCart();
            ChangeFullEmptyCart();
            SetSelectedProdSumQuantity();
        }
        private void OnSaveCart(object sender, EventArgs e) => SaveCart();
        private ObservableCollection<Carts> GetCarts() =>
         new ObservableCollection<Carts>(App.ctx.Carts.Where(c => c.UserID == mainWindow.UserID).ToList());
        private void SetGroupCheck() => chbAll.IsChecked = !cart.Any(c => c.IsChecked != true);
        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Carts.IsChecked))
            {
                SetGroupCheck();
                SetSelectedProdSumQuantity();
            }
        }
        private void SetSelectedProdSumQuantity()
        {
            lblTotalSum.Content = cart.Where(c => c.IsChecked).Sum(c => c.SumProducts).ToString();
            lblTotalQuantity.Content = cart.Where(p => p.IsChecked).Sum(p => p.QuantityCart).ToString();
            txtBlIsSelected.Text = cart.Count(p => p.IsChecked).ToString();
        }
        private void ResetCart()
        {
            cart = GetCarts();
            foreach (Carts crt in cart)
                crt.PropertyChanged += CartItem_PropertyChanged;
            ICCart.ItemsSource = cart;
        }
        private void SaveCart()
        {
            try
            {
                App.ctx.SaveChanges();
                mainWindow.UpdateCartState();
                SetSelectedProdSumQuantity();
            }
            catch (Exception ex)
            {
                ResetCart();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
        private void ChangeFullEmptyCart()
        {
            if (App.ctx.Carts.FirstOrDefault(c => c.UserID == mainWindow.UserID) != null)
            {
                stPEmpty.Visibility = Visibility.Collapsed;
                GrTopPan.Visibility = Visibility.Visible;
                ICCart.Visibility = Visibility.Visible;
                GrBottomPan.Visibility = Visibility.Visible;
            }
            else
            {
                stPEmpty.Visibility = Visibility.Visible;
                GrTopPan.Visibility = Visibility.Hidden;
                ICCart.Visibility = Visibility.Hidden;
                GrBottomPan.Visibility = Visibility.Hidden;
            }
        }
        private List<Carts> GetSortedProd(string cmbName)
        {
            switch (cmbName)
            {
                case "cmbIAZ":
                    return cart.OrderBy(p => p.Products.ProductName).ToList();
                case "cmbIZA":
                    return cart.OrderByDescending(p => p.Products.ProductName).ToList();
                case "cmbILowPrice":
                    return cart.OrderBy(p => p.SumProducts).ToList();
                case "cmbIHighPrice":
                    return cart.OrderByDescending(p => p.SumProducts).ToList();
                case "cmbIAvail":
                    return cart.OrderBy(p => p.Products.AvailabilityStatusID).ToList();
                case "cmbINotAvail":
                    return cart.OrderByDescending(p => p.Products.AvailabilityStatusID).ToList();
                default:
                    return cart.ToList();
            }
        }
        private void SortProd(string cmbName)
        {
            ICCart.ItemsSource = GetSortedProd(cmbName);
        }
        private void GoAboutProduct(Carts cart)
        {
            this.NavigationService
                .Navigate(new OneProductWithoutFeedbackPage(App.ctx.Products.Where(c => c.ProductID == cart.ProductID).FirstOrDefault()));
        }
        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Hyperlink)sender).DataContext;
            GoAboutProduct(selectedCartProd);
        }
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortProd((((ComboBox)sender).SelectedItem as ComboBoxItem).Name);
        }
        private void MakingOrderOneProduct(Carts cart)
        {
            List<Carts> crt = new List<Carts>() { cart };
            this.NavigationService.Navigate(new OrderRegistrationPage(crt));
        }
        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            MakingOrderOneProduct(selectedCartProd);
        }
        private void ChangeQuantity(Carts cr, RepeatButton rb, bool isIncrement = true)
        {
            cr.Quantity = isIncrement? ++cr.Quantity : --cr.Quantity;
            saveTimer.Stop();
            saveTimer.Start();
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            ChangeQuantity(selectedCartProd, (RepeatButton)sender, false);
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((RepeatButton)sender).DataContext;
            ChangeQuantity(selectedCartProd, (RepeatButton)sender);
        }
        private void DelOneProduct(Carts cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить выбранный товар из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            App.ctx.Carts.Remove(cr);
            cart.Remove(cr);
            ICCart.ItemsSource = cart;
            SaveCart();
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Carts selectedCartProd = (Carts)((Button)sender).DataContext;
            DelOneProduct(selectedCartProd);
        }
        private void DelSelectedProduct(List<Carts> cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Удалить выбранные товары из корзины?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgInf != MessageBoxResult.Yes)
                return;
            App.ctx.Carts.RemoveRange(cr);
            foreach(Carts crt in cr)
                cart.Remove(crt);
            ICCart.ItemsSource = cart;
            SaveCart();
        }
        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            DelSelectedProduct(cart.Where(c => c.IsChecked).ToList());
        }
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            List<Carts> crt = cart.Where(c => c.IsChecked).ToList();
            if(crt.Count == 0)
            {
                MessageBox.Show("Выберите товары для формирования заказа");
                return;
            }
            this.NavigationService.Navigate(new OrderRegistrationPage(crt));
        }
        private void btnEmptyBuy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }
        private void GroupSelect(bool IsChecked = true)
        {
            foreach (Carts crt in cart)
                crt.IsChecked = IsChecked;
        }
        private void chbAll_Click(object sender, RoutedEventArgs e)
        {
            GroupSelect(((CheckBox)sender).IsChecked == true);
        }
        private void RepeatButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (cmbILowPrice.IsSelected || cmbIHighPrice.IsSelected)
                SortProd(((ComboBoxItem)cmbSort.SelectedItem).Name);
        }
        private void RepeatButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((cmbILowPrice.IsSelected || cmbIHighPrice.IsSelected) && !(bool)e.NewValue)
                SortProd(((ComboBoxItem)cmbSort.SelectedItem).Name);
        }
    }
}
