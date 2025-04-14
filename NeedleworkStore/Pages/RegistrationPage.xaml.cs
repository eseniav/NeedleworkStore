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
        bool isError = false;
        public RegistrationPage()
        {
            InitializeComponent();
            txtBlLog.Text = "Минимальная длина - 3 символа. " +
                "Нельзя использовать символы:\n& + = < > , \" \' ` ~";
            txtBlPass.Text = "Минимальная длина - 8 символов. " +
                "Пароль должен содержать хотя бы 1 цифру и заглавную букву.";
            txtBlDate.Text = "Регистрация возможна с 14 лет";
            txtBlReqFields.Text = "Поля с * обязательны для заполнения";            
        }
        private void ColorTextBox(TextBox tb, bool isError)
        {
            if (isError)
                tb.Background = new SolidColorBrush(Color.FromArgb(100, 251, 174, 210));
            else
                tb.Background = new SolidColorBrush(Colors.White);
        }
        private void ColorTextBox(TextBox tb, bool isError, TextBlock tbl)
        {
            if (isError)
                tb.Background = new SolidColorBrush(Color.FromArgb(100, 251, 174, 210));
            else
            {
                tbl.Text = string.Empty;
                tb.Background = new SolidColorBrush(Colors.White);
            }
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckValidation.CheckEmptyNull(txtBLogin.Text) || !CheckValidation.CheckEmptyNull(psxB.Password)
                        || !CheckValidation.CheckEmptyNull(pswBRepeat.Password) || !CheckValidation.CheckEmptyNull(txtBPhone.Text)
                        || !CheckValidation.CheckEmptyNull(txtBEmail.Text))
                {
                    MessageBox.Show("Обязательные поля должны быть заполнены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!CheckValidation.CheckLogin(txtBLogin.Text) || !CheckValidation.CheckPass(psxB.Password)
                    || !CheckValidation.CheckPhone(txtBPhone.Text) || !CheckValidation.CheckEmail(txtBEmail.Text)
                    || !CheckValidation.CheckNames(txtBLastName.Text) || !CheckValidation.CheckNames(txtBFirstName.Text)
                    || !CheckValidation.CheckNames(txtBPatr.Text))
                {
                    MessageBox.Show("Ошибка заполнения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (psxB.Password != pswBRepeat.Password)
                {
                    MessageBox.Show("Введенные пароли не совпадают", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (dtPBirth.SelectedDate != null && !CheckValidation.CheckDate(dtPBirth.SelectedDate.Value))
                {
                    MessageBox.Show("Неверно выбрана дата", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }                
                Users newUser = new Users
                {
                    UserLastname = txtBLastName.Text,
                    UserName = txtBFirstName.Text,
                    UserPatronymic = txtBPatr.Text,
                    Login = txtBLogin.Text,
                    Password = psxB.Password,
                    UserPhone = CheckValidation.CorrectPhone(),
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
                return;
            }
        }

        private void txtBLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(txtBLogin.Text))
            {
                errorLog.Text = "Поле обязательно для заполнения";
                isError = true;                
            }
            if (!CheckValidation.CheckLogin(txtBLogin.Text))
            {
                errorLog.Text = "Поле заполнено неправильно";
                isError = true;
            }
            if (App.ctx.Users.FirstOrDefault(u => u.Login == txtBLogin.Text) != null)
            {
                errorLog.Text = "Введенный логин уже используется";
                isError = true;
            }
            ColorTextBox(txtBLogin, isError, errorLog);
        }

        private void psxB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(psxB.Password))
            {
                errorPass.Text = "Поле обязательно для заполнения";                
                isError = true;                
            }
            if (!CheckValidation.CheckPass(psxB.Password))
            {
                errorPass.Text = "Поле заполнено неправильно";
                isError = true;                
            }
            if (isError)
                psxB.Background = new SolidColorBrush(Color.FromArgb(100, 251, 174, 210));
            else
            {
                errorPass.Text = string.Empty;
                psxB.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void pswBRepeat_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(pswBRepeat.Password))
            {
                isError = true;
                errorRepPass.Text = "Поле обязательно для заполнения";                
            }
            if(psxB.Password != pswBRepeat.Password)
            {
                isError = true;
                errorRepPass.Text = "Пароли не совпадают";                
            }
            if (isError)
                pswBRepeat.Background = new SolidColorBrush(Color.FromArgb(100, 251, 174, 210));
            else
            {
                errorRepPass.Text = string.Empty;
                pswBRepeat.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void txtBPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckValidation.CheckEmptyNull(txtBPhone.Text))
                {
                    isError = true;
                    errorPhone.Text = "Поле обязательно для заполнения";
                }
                if (!CheckValidation.CheckPhone(txtBPhone.Text))
                {
                    isError = true;
                    errorPhone.Text = "Телефон должен начинаться с +7 или 8 и содержать 11 цифр";
                }
                else if (App.ctx.Users.FirstOrDefault(u => u.UserPhone == CheckValidation.CorrectPhone()) != null)
                {
                    isError = true;
                    errorPhone.Text = "На этот телефон уже зарегистрирован аккаунт";
                }
                ColorTextBox(txtBPhone, isError, errorPhone);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtBEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckEmptyNull(txtBEmail.Text))
            {
                isError = true;
                errorEmail.Text = "Поле обязательно для заполнения";
            }
            if(!CheckValidation.CheckEmail(txtBEmail.Text))
            {
                isError = true;
                errorEmail.Text = "В Email должны присутствовать @ и .";
            }
            if (App.ctx.Users.FirstOrDefault(u => u.UserEmail == txtBEmail.Text) != null)
            {
                isError = true;
                errorPhone.Text = "На этот email уже зарегистрирован аккаунт";
            }
            ColorTextBox(txtBEmail, isError, errorEmail);
        }

        private void dtPBirth_LostFocus(object sender, RoutedEventArgs e)
        {
            if(dtPBirth.SelectedDate != null)
            {
                if (!CheckValidation.CheckDate(dtPBirth.SelectedDate.Value))
                {
                    isError = true;
                    errorDate.Text = "Неверно выбрана дата";
                }
                if (isError)
                    dtPBirth.Background = new SolidColorBrush(Color.FromArgb(100, 251, 174, 210));
                else
                {
                    errorDate.Text = string.Empty;
                    dtPBirth.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void txtBFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!CheckValidation.CheckNames(txtBFirstName.Text))                
                isError = true;
            ColorTextBox(txtBFirstName, isError);
        }
    }
}
