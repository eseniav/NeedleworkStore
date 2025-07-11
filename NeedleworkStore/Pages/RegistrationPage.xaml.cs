﻿using NeedleworkStore.AppData;
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
                "Допустимы только латинские буквы и цифры.";
            txtBlPass.Text = "Длина от 8 до 50 символов. " +
                "Пароль должен содержать хотя бы 1 цифру и заглавную букву.";
            txtBlDate.Text = "Регистрация возможна с 14 лет.";
            txtBlReqFields.Text = "Поля с * обязательны для заполнения.";
            mainWindow.btnProd.IsEnabled = true;
        }
        public bool CheckLoginError()
        {
            ValidationState state = CheckValidation.CheckLogin(txtBLogin);
            SetLayout.ColorInputControl(txtBLogin, state.IsError);
            SetLayout.SetErrorText(errorLog, state);
            return state.IsError;
        }
        private bool CheckAllBlocks()
        {
            return CheckLoginError() || CheckPassError() || CheckRepeatPassError() || CheckPhoneError() || CheckLNError() || CheckEmailError() ||
                CheckFNError() || CheckPatrError() || CheckDateError();
        }
        private void CheckEmpty()
        {
            CheckLoginError();
            CheckPassError();
            CheckRepeatPassError();
            CheckPhoneError();
            CheckEmailError();
        }
        private void CheckPhoneExistInDB()
        {
            ValidationState state = new ValidationState();
            string correctPhone = CheckValidation.CorrectPhone(txtBPhone.Text);
            if (App.ctx.Users.FirstOrDefault(u => u.UserPhone == correctPhone) != null)
            {
                state.SetError("На этот телефон уже зарегистрирован аккаунт");
                SetLayout.ColorInputControl(txtBPhone, state.IsError);
                errorPhone.Text = state.ErrorMessage;
            }
        }
        private void CheckEmailExistInDB()
        {
            ValidationState state = new ValidationState();
            if (App.ctx.Users.FirstOrDefault(u => u.UserEmail == txtBEmail.Text) != null)
            {
                state.SetError("На этот email уже зарегистрирован аккаунт");
                SetLayout.ColorInputControl(txtBEmail, state.IsError);
                errorEmail.Text = state.ErrorMessage;
            }
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            CheckEmpty();
            if (CheckValidation.CheckEmptyNull(txtBPhone.Text))
                CheckPhoneExistInDB();
            if (CheckValidation.CheckEmptyNull(txtBEmail.Text))
                CheckEmailExistInDB();
            if (CheckAllBlocks())
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
                MessageBox.Show("Регистрация прошла успешно!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new AuthPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool CheckPassError()
        {
            ValidationState state = CheckValidation.CheckPassword(psxB);
            SetLayout.ColorInputControl(psxB, state.IsError);
            SetLayout.SetErrorText(errorPass, state);
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
            SetLayout.ColorInputControl(pswBRepeat, state.IsError);
            SetLayout.SetErrorText(errorRepPass, state);
            return state.IsError;
        }
        private void pswBRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckRepeatPassError();
        }
        public bool CheckPhoneError()
        {
            ValidationState state = CheckValidation.CheckPhone(txtBPhone);
            SetLayout.ColorInputControl(txtBPhone, state.IsError);
            SetLayout.SetErrorText(errorPhone, state);
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
                e.Handled = true;
        }
        public bool CheckEmailError()
        {
            ValidationState state = CheckValidation.CheckEmail(txtBEmail);
            SetLayout.ColorInputControl(txtBEmail, state.IsError);
            SetLayout.SetErrorText(errorEmail, state);
            return state.IsError;
        }
        private void txtBEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmailError();
        }
        public bool CheckCyrillicError(TextBox textBox, TextBlock errorTextBlock)
        {
            ValidationState state = CheckValidation.CheckCyrillic(textBox);
            SetLayout.ColorInputControl(textBox, state.IsError);
            SetLayout.SetErrorText(errorTextBlock, state);
            return state.IsError;
        }
        public bool CheckLNError() => CheckCyrillicError(txtBLastName, errorLN);
        private void txtBLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLNError();
        }
        public bool CheckFNError() => CheckCyrillicError(txtBFirstName, errorFN);
        private void txtBFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFNError();
        }
        public bool CheckPatrError() => CheckCyrillicError(txtBPatr, errorPatr);
        private void txtBPatr_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPatrError();
        }
        public bool CheckDateError()
        {
            ValidationState state = CheckValidation.CheckBirthDate(dtPBirth);
            SetLayout.ColorInputControl(dtPBirth, state.IsError);
            SetLayout.SetErrorText(errorDate, state);
            return state.IsError;
        }
        private void dtPBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDateError();
        }
    }
}
