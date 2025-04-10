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
    /// Логика взаимодействия для OneProductWithoutFeedbackPage.xaml
    /// </summary>
    public partial class OneProductWithoutFeedbackPage : Page
    {
        public OneProductWithoutFeedbackPage()
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
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Переход на страницу с этим товаром");
        }
        private void imQR_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Переход на страницу сайта с готовыми работами");
        }

        private void hlThemes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Переход на страницу с отфильтрованными товарами по каждой теме");
        }

        private void rbtnColor_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Выбор соответствующего цвета");
            if (((RadioButton)e.Source).IsChecked == true)
            {

            }
        }
        private void cmbRating_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Выбор рейтинга");
        }

        private void ImageDel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Удаляет картинку, предварительно узнав о необходимости");
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Диалоговое окно для добавления картинки");
        }

        private void btnAddFeedback_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Публикует отзыв");
        }
    }
}
