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
            txtBlPass.Text = "Минимальная длина - 8 символов. " +
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
                : new SolidColorBrush(Colors.White);
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
            CheckLoginError();
            CheckPassError();
            if (CheckLoginError() || CheckPassError())
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
                    UserPhone = txtBPhone.Text,
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
    }
}
