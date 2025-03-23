﻿using NeedleworkStore.Classes;
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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
        }
        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }
            else
            {
                MessageBox.Show("No page to go forward");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No page to go back");
            }
        }
        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу О магазине");
        }

        private void btnProd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AuthPage());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation.CheckEmptyNull(txtSearch.Text))
            {
                MessageBox.Show("Переход на страницу с найденной информацией\n" +
                "или подсвеченная информация на этой странице");
            }
            else
            {
                MessageBox.Show("Заполните поле!");
            }
        }
        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CartPage());
        }

        private void btnFav_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Избранное");
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Заказы");
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Редактирование профиля");
        }
    }
}
