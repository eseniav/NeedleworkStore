using NeedleworkStore.AppData;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public RegistrationPage()
        {
            InitializeComponent();
            txtBlLog.Text = "Минимальная длина - 3 символа. " +
                "Нельзя использовать: & + = < > , \" \' ` ~";
            txtBlPass.Text = "Длина от 8 до 50 символов. " +
                "Пароль должен содержать хотя бы 1 цифру и заглавную букву.";
            txtBlDate.Text = "Регистрация возможна с 14 лет";
            txtBlReqFields.Text = "Поля с * обязательны для заполнения";
            mainWindow.btnProd.IsEnabled = true;
        }
        private void ColorInputControl(Control control, bool isError)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            var brush = isError
                ? new SolidColorBrush(Color.FromArgb(100, 251, 174, 210))
                : Brushes.White;
            control.Background = brush;
        }
        public bool CheckLoginError()
        {
            ValidationState state = CheckValidation.CheckLogin(txtBLogin);
            ColorInputControl(txtBLogin, state.IsError);
            errorLog.Text = state.ErrorMessage;
            return state.IsError;
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            ValidationState state = new ValidationState();
            CheckLoginError();
            CheckPassError();
            CheckRepeatPassError();
            CheckPhoneError();
            CheckEmailError();
            if (CheckValidation.CheckEmptyNull(txtBPhone.Text))
            {
                string correctPhone = CheckValidation.CorrectPhone(txtBPhone.Text);
                if (App.ctx.Users.FirstOrDefault(u => u.UserPhone == correctPhone) != null)
                {
                    state.SetError("На этот телефон уже зарегистрирован аккаунт");
                    ColorInputControl(txtBPhone, state.IsError);
                    errorPhone.Text = state.ErrorMessage;
                }
            }
            if (CheckValidation.CheckEmptyNull(txtBEmail.Text))
            {
                if (App.ctx.Users.FirstOrDefault(u => u.UserEmail == txtBEmail.Text) != null)
                {
                    state.SetError("На этот email уже зарегистрирован аккаунт");
                    ColorInputControl(txtBEmail, state.IsError);
                    errorEmail.Text = state.ErrorMessage;
                }
            }
            if (CheckLoginError() || CheckPassError() || CheckRepeatPassError() || CheckPhoneError())
            {
                MessageBox.Show("Проверьте форму на ошибки!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                Users newUser = new Users
                {
                    UserLastname = txtBLastName.Text,
                    UserName = txtBFirstName.Text,
                    UserPatronymic = txtBPatr.Text,
                    Login = txtBLogin.Text,
                    Password = psxB.Password,
                    UserPhone = CheckValidation.CorrectPhone(txtBPhone.Text),
                    UserEmail = txtBEmail.Text,
                    Birthday = dtPBirth.SelectedDate,
                    RoleID = 2
                };
                App.ctx.Users.Add(newUser);
                App.ctx.SaveChanges();
                this.NavigationService.Navigate(new ProductsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CheckPassError()
        {
            ValidationState state = CheckValidation.CheckPassword(psxB);
            ColorInputControl(psxB, state.IsError);
            errorPass.Text = state.ErrorMessage;
            return state.IsError;
        }
        private void txtBLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLoginError();
        }
        private void psxB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPassError();
        }
        public bool CheckRepeatPassError()
        {
            ValidationState state = CheckValidation.CheckRepeatPass(pswBRepeat);
            if (psxB.Password != pswBRepeat.Password)
                state.SetError("Пароли не совпадают");
            ColorInputControl(pswBRepeat, state.IsError);
            errorRepPass.Text = state.ErrorMessage;
            return state.IsError;
        }
        private void pswBRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckRepeatPassError();
        }
        public bool CheckPhoneError()
        {
            ValidationState state = CheckValidation.CheckPhone(txtBPhone);
            ColorInputControl(txtBPhone, state.IsError);
            errorPhone.Text = state.ErrorMessage;
            return state.IsError;
        }
        public void SetPhoneFormat(TextBox textBox)
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
        private void txtBPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPhoneError();
            TextBox textBox = sender as TextBox;
            if (e.Changes.Any(change => change.AddedLength > 1))
                return;
            SetPhoneFormat(textBox);
        }

        private void txtBPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void txtBPhone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.Key == Key.Back && textBox.SelectionStart <= 4)
            {
                e.Handled = true;
            }
        }
        public bool CheckEmailError()
        {
            ValidationState state = CheckValidation.CheckEmail(txtBEmail);
            ColorInputControl(txtBEmail, state.IsError);
            errorEmail.Text = state.ErrorMessage;
            return state.IsError;
        }
        private void txtBEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmailError();
        }
    }
}
