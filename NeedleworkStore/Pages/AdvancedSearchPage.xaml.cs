using NeedleworkStore.Classes;
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
    /// Логика взаимодействия для AdvancedSearchPage.xaml
    /// </summary>
    public partial class AdvancedSearchPage : Page
    {
        public AdvancedSearchPage()
        {
            InitializeComponent();
            txtFrom.Focus();
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

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу Профиль");
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

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if ((CheckValidation.CheckInt(txtFrom.Text) || !CheckValidation.CheckEmptyNull(txtFrom.Text)) && 
                (CheckValidation.CheckInt(txtTo.Text) || !CheckValidation.CheckEmptyNull(txtTo.Text)))
            {
                MessageBox.Show("Переход на страницу с отфильтрованными товарами");                                                
                return;
            }
            else
            {
                MessageBox.Show("Введены неверные данные!");
            }            
        }
    }
}
