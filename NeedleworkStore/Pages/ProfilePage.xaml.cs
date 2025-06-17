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
                if (item.Key == "txtPhone" && !string.IsNullOrEmpty(item.Value))
                    FormatPhoneForTextBlock(textBlock);
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
        public static void FormatPhoneForTextBlock(TextBlock textBlock)
        {
            if (string.IsNullOrEmpty(textBlock.Text))
                return;

            string digitsOnly = new string(textBlock.Text.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length == 10)
            {
                textBlock.Text = $"+7 ({digitsOnly.Substring(0, 3)}) " +
                                $"{digitsOnly.Substring(3, 3)} " +
                                $"{digitsOnly.Substring(6, 2)} " +
                                $"{digitsOnly.Substring(8)}";
            }
        }
        public ProfilePage()
        {
            InitializeComponent();
            FormatPhoneForTextBlock(txtPhone);
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
            mainWindow.btnAddProd.IsEnabled = true;
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
                if (!string.IsNullOrEmpty(user?.UserPhone))
                {
                    boxPhone.Text = "+7 (" + user.UserPhone.Substring(0, 3) + ") " +
                                   user.UserPhone.Substring(3, 3) + " " +
                                   user.UserPhone.Substring(6, 2) + " " +
                                   user.UserPhone.Substring(8);
                }
            }
        }
        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            ChangeStartRedactionPages();
            SetRegistrationValues();
            FormatPhoneForTextBox(boxPhone);
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
        private bool CheckAllBlocks()
        {
            return CheckLoginError() || CheckPassError() || CheckRepeatPassError() || CheckPhoneError() || CheckLNError() || CheckEmailError() ||
                CheckFNError() || CheckPatrError() || CheckDateError();
        }
        private void btnSavechanges_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(boxPass2.Password))
            {
                MessageBox.Show("Для сохранения данных введит пароль повторно", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (CheckAllBlocks())
            {
                MessageBox.Show("Проверьте форму на ошибки!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                user.Login = boxLog.Text;
                user.UserEmail = boxEmail.Text;
                user.UserLastname = boxLastName.Text;
                user.UserName = boxFirstname.Text;
                user.UserPatronymic = boxPatr.Text;
                user.UserPhone = CheckValidation.CorrectPhone(boxPhone.Text);
                user.Password = boxPass.Password;
                if (boxBirthDate.SelectedDate.HasValue)
                    user.Birthday = boxBirthDate.SelectedDate.Value;
                else
                    user.Birthday = null;
                App.ctx.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new ProfilePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CheckLoginError()
        {
            ValidationState state = new ValidationState();
            if (boxLog.Text == user.Login)
                return state.IsError;
            state = CheckValidation.CheckLogin(boxLog);
            SetLayout.ColorInputControl(boxLog, state.IsError);
            return state.IsError;
        }
        private void boxLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLoginError();
        }
        public bool CheckPassError()
        {
            ValidationState state = new ValidationState();
            if (boxPass.Password == user.Password)
                return state.IsError;
            state = CheckValidation.CheckPassword(boxPass);
            SetLayout.ColorInputControl(boxPass, state.IsError);
            return state.IsError;
        }
        private void boxPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPassError();
        }
        public bool CheckRepeatPassError()
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(boxPass2.Password))
                return state.IsError;
            state = CheckValidation.CheckRepeatPass(boxPass2);
            if (boxPass.Password != boxPass2.Password)
                state.IsError = true;
            SetLayout.ColorInputControl(boxPass2, state.IsError);
            return state.IsError;
        }
        private void boxPass2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckRepeatPassError();
        }
        public bool CheckEmailError()
        {
            ValidationState state = new ValidationState();
            if (boxEmail.Text == user.UserEmail)
                return state.IsError;
            state = CheckValidation.CheckEmail(boxEmail);
            SetLayout.ColorInputControl(boxEmail, state.IsError);
            return state.IsError;
        }
        private void boxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmailError();
        }
        public bool CheckCyrillicError(TextBox textBox)
        {
            ValidationState state = CheckValidation.CheckCyrillic(textBox);
            SetLayout.ColorInputControl(textBox, state.IsError);
            return state.IsError;
        }
        public bool CheckLNError()
        {
            ValidationState state = new ValidationState();
            if (boxLastName.Text == user.UserLastname)
                return state.IsError;
            return CheckCyrillicError(boxLastName);
        }
        private void boxLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLNError();
        }
        public bool CheckFNError()
        {
            ValidationState state = new ValidationState();
            if (boxFirstname.Text == user.UserName)
                return state.IsError;
            return CheckCyrillicError(boxFirstname);
        }
        private void boxFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFNError();
        }
        public bool CheckPatrError()
        {
            ValidationState state = new ValidationState();
            if (boxPatr.Text == user.UserPatronymic)
                return state.IsError;
            return CheckCyrillicError(boxPatr);
        }
        private void boxPatr_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPatrError();
        }
        public bool CheckDateError()
        {
            ValidationState state = new ValidationState();
            if (boxBirthDate.SelectedDate == user.Birthday)
                return state.IsError;
            state = CheckValidation.CheckBirthDate(boxBirthDate);
            SetLayout.ColorInputControl(boxBirthDate, state.IsError);
            return state.IsError;
        }
        private void boxBirthDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDateError();
        }
        public bool CheckPhoneError()
        {
            ValidationState state = new ValidationState();
            if (CheckValidation.CorrectPhone(boxPhone.Text) == user.UserPhone)
                return state.IsError;
            state = CheckValidation.CheckPhone(boxPhone);
            SetLayout.ColorInputControl(boxPhone, state.IsError);
            return state.IsError;
        }
        public static void FormatPhoneForTextBox(TextBox textBox)
        {
            string text = textBox.Text;
            string digitsOnly = new string(text.Where(c => char.IsDigit(c) || c == '+').ToArray());
            if (digitsOnly.StartsWith("+7") && digitsOnly.Length > 2)
            {
                string numbers = digitsOnly.Substring(2);
                string formatted = "+7 (";

                if (numbers.Length > 0)
                    formatted += numbers.Length > 3 ? numbers.Substring(0, 3) + ") " : numbers + ") ";
                if (numbers.Length > 3)
                    formatted += numbers.Length > 6 ? numbers.Substring(3, 3) + " " : numbers.Substring(3);
                if (numbers.Length > 6)
                    formatted += numbers.Length > 8 ? numbers.Substring(6, 2) + " " : numbers.Substring(6);
                if (numbers.Length > 8)
                    formatted += numbers.Substring(8);

                textBox.Text = formatted.Trim();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
        private void boxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPhoneError();
            TextBox textBox = sender as TextBox;
            if (e.Changes.Any(change => change.AddedLength > 1))
                return;
            FormatPhoneForTextBox(textBox);
        }
        private void boxPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
            if (textBox.Text.Length == 0)
            {
                textBox.Text = "+7 (";
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
        private void boxPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.Key == Key.Back && textBox.SelectionStart <= 4)
                e.Handled = true;
        }
    }
}
