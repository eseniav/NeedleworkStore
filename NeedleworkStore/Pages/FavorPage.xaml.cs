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
using System.Windows.Threading;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для FavorPage.xaml
    /// </summary>
    public partial class FavorPage : Page
    {
        public FavorPage()
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
        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CartPage());
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

        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу О магазине");
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProfilePage());
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

        private void btnStitch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами Вышивка");
        }

        private void btnSew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами Шитье");
        }

        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами Аксессуары");
        }

        private void btnKits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами Наборы");
        }

        private void btnPrice_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(txtFrom.Text) && !CheckValidation.CheckEmptyNull(txtTo.Text))
            {
                MessageBox.Show("Хотя бы одно поле должно быть заполнено!");
                return;
            }
            if ((CheckValidation.CheckInt(txtFrom.Text) || !CheckValidation.CheckEmptyNull(txtFrom.Text)) &&
                (CheckValidation.CheckInt(txtTo.Text) || !CheckValidation.CheckEmptyNull(txtTo.Text)))
            {
                MessageBox.Show("Переход на страницу с отфильтрованными товарами по диапазону цены");
                return;
            }
            else
            {
                MessageBox.Show("Введены неверные данные!");
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Соответствующая сортировка");
        }

        private void lbxDesigners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждому производителю");
        }

        private void lbxThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждой теме");
        }

        private void lbxTechnic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждой технике");
        }

        private void hlAbout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductPage());
        }
        private void txtFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFrom.SelectAll();
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет конкретный товар.\nПеред удалением появляется специальное окошко с выбором");
        }
        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удаляет выбранные товары.\nПеред удалением появляется специальное окошко с выбором");
        }
    }
}
