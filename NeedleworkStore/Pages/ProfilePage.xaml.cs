using NeedleworkStore.AppData;
using NeedleworkStore.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        Users user;
        List<UIElement> Start;
        List<UIElement> Redaction;
        private void SetProfileValues()
        {
            Dictionary<string, string> profileData = new Dictionary<string, string>
            {
                { "txtLog", user.Login },
                { "txtEmail", user.UserEmail },
                { "txtLastname", user.UserLastname },
                { "txtFirstname", user.UserName },
                { "txtPatr", user.UserPatronymic },
                { "txtBirthDate", user.Birthday?.ToString("dd.MM.yyyy") },
                { "txtPhone", user.UserPhone },
            };
            string defaultText = "Данные не указаны";
            SolidColorBrush defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#852614"));
            foreach (var item in profileData)
            {
                TextBlock textBlock = (TextBlock)FindName(item.Key);
                textBlock.Text = string.IsNullOrEmpty(item.Value) ? defaultText : item.Value;
                textBlock.Foreground = string.IsNullOrEmpty(item.Value) ? defaultColor : textBlock.Foreground;
            }
        }
        public void ChangeStartRedactionPages()
        {
            Start.ForEach(el => el.Visibility = Visibility.Collapsed);
            Redaction.ForEach(el => el.Visibility = Visibility.Visible);
        }
        public void ChangeRedactionStartPages()
        {
            Redaction.ForEach(el => el.Visibility = Visibility.Collapsed);
            Start.ForEach(el => el.Visibility = Visibility.Visible);
        }
        public void SetAdminPanel() => LeftPanel.Visibility = mainWindow.RoleID == 1 ? Visibility.Hidden : Visibility.Visible;
        public ProfilePage()
        {
            InitializeComponent();
            user = App.ctx.Users.FirstOrDefault(u => u.UserID == mainWindow.UserID);
            SetProfileValues();
            SetAdminPanel();
            Start = new List<UIElement> {
                    txtLog,
                    txtEmail,
                    txtLastname,
                    txtFirstname,
                    txtPatr,
                    txtBirthDate,
                    txtPhone,
                    txtPass,
                    chbShowPass,
                    txtShowPass,
                    btnRedact,
                };
            Redaction = new List<UIElement>
                {
                    boxLog,
                    boxPass,
                    boxPass2,
                    txtRepeatPass,
                    boxEmail,
                    boxLastName,
                    boxFirstname,
                    boxPatr,
                    boxBirthDate,
                    boxPhone,
                    btnBackProfile,
                    BottomPanel,
                };
            ChangeRedactionStartPages();
            mainWindow.btnProd.IsEnabled = true;
        }
        private void btnFav_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage(null, false));
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrdersPage());
        }
        private void SetRegistrationValues()
        {
            Dictionary<string, string> registrationData = new Dictionary<string, string>
            {
                { "boxLog", user?.Login },
                { "boxPass", user?.Password },
                { "boxEmail", user?.UserEmail },
                { "boxLastName", user?.UserLastname },
                { "boxFirstname", user?.UserName },
                { "boxPatr", user?.UserPatronymic },
                { "boxBirthDate", user?.Birthday?.ToString("yyyy-MM-dd") },
                { "boxPhone", user?.UserPhone }
            };

            string defaultText = "";
            Brush defaultPlaceholderBrush = new SolidColorBrush(Colors.Gray);

            foreach (var item in registrationData)
            {
                var control = FindName(item.Key);

                if (control is TextBox textBox)
                {
                    textBox.Text = string.IsNullOrEmpty(item.Value) ? defaultText : item.Value;
                    textBox.Foreground = string.IsNullOrEmpty(item.Value) ? defaultPlaceholderBrush : Brushes.Black;
                }
                else if (control is PasswordBox passwordBox)
                {
                    passwordBox.Password = item.Value ?? defaultText;
                }
                else if (control is DatePicker datePicker && DateTime.TryParse(item.Value, out var date))
                {
                    datePicker.SelectedDate = date;
                }
            }
        }
        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            ChangeStartRedactionPages();
            SetRegistrationValues();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new OneProductWithoutFeedbackPage((Products)((Hyperlink)sender).DataContext));
        }
        private void chbShowPass_Click(object sender, RoutedEventArgs e)
        {
            txtPass.Text = (bool)chbShowPass.IsChecked ? user.Password : "********";
        }

        private void btnBackProfile_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProfilePage());
        }
        public bool CheckValid()
        {
            if (string.IsNullOrWhiteSpace(boxLog.Text))
            {
                MessageBox.Show("Логин не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (boxLog.Text != user.Login &&
                App.ctx.Users.Any(u => u.Login == boxLog.Text))
            {
                MessageBox.Show("Этот логин уже занят!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false; ;
            }
            if (!string.IsNullOrWhiteSpace(boxPass.Text))
                {
                if (boxPass.Text != boxPass2.Text)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (boxPass.Text.Length < 8)
                {
                    MessageBox.Show("Пароль должен содержать минимум 8 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }
        private void btnSavechanges_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValid())
                return;
            try
            {
                user.Login = boxLog.Text;
                user.UserEmail = boxEmail.Text;
                user.UserLastname = boxLastName.Text;
                user.UserName = boxFirstname.Text;
                user.UserPatronymic = boxPatr.Text;
                user.UserPhone = boxPhone.Text;
                if (!string.IsNullOrWhiteSpace(boxPass.Text))
                {
                    user.Password = boxPass.Text;
                }
                if (boxBirthDate.SelectedDate.HasValue)
                {
                    user.Birthday = boxBirthDate.SelectedDate.Value;
                }
                else
                {
                    user.Birthday = null;
                }
                App.ctx.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new ProfilePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
