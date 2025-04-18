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
using System.Windows.Threading;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            if (Int32.Parse(txblQuan.Text.ToString()) == 1)
            {
                btnMinus.IsEnabled = false;
            }
            if (Int32.Parse(txblQuan.Text.ToString()) == 99)
            {                
                btnPlus.IsEnabled = false;
            }
        }
               
        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Соответствующая сортировка");
        }
        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу оформления заказа только с этой позицией");
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

        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет выбранные товары.\nПеред удалением появляется специальное окошко с выбором");
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Если не выбрано ничего - автоматически выбирает все товары\n" +
                "Если выбраны определенные товары - формирует заказ из них");
        }
        
    }
}
