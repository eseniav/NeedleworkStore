using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace NeedleworkStore.Classes
{
    static internal class CheckValidation
    {      
        public static bool CheckLogin(string txt)
        {
            if (txt.Length < 3)
                return false;
            char[] forbiddenChars = { '&', '+', '=', '<', '>', ',', '-', '"', '\'' };
            return !txt.Any(c => forbiddenChars.Contains(c));
        }
        public static ValidationState CheckLogin(TextBox txtBLogin)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(txtBLogin.Text))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckLogin(txtBLogin.Text))
                state.SetError("Поле заполнено неправильно");
            else if (App.ctx.Users.FirstOrDefault(u => u.Login == txtBLogin.Text) != null)
                state.SetError("Введенный логин уже используется");
            return state;
        }
        public static bool CheckPass(string txt)
        {
            if (txt.Length < 8 || txt.Length > 50)
                return false;
            bool hasCapital = false;
            bool hasNumber = false;
            foreach (char ch in txt)
            {
                if (char.IsUpper(ch))
                    hasCapital = true;
                else if (char.IsDigit(ch))
                    hasNumber = true;
                if (hasCapital && hasNumber)
                    return true;
            }
            return hasCapital && hasNumber;
        }
        public static ValidationState CheckPassword(PasswordBox psxB)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(psxB.Password))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckPass(psxB.Password))
                state.SetError("Поле заполнено неправильно");
            return state;
        }
        public static ValidationState CheckRepeatPass(PasswordBox pswBRepeat)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(pswBRepeat.Password))
                state.SetError("Поле обязательно для заполнения");
            return state;
        }
        public static bool CheckPhone(string txt)
        {
            if (txt.Length != 11)
                return false;
            return true;
        }
        public static string CorrectPhone(string txt)
        {
            string digitsOnly = new string(txt.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length == 1)
                return digitsOnly;
            return digitsOnly.Substring(1);
        }
        public static ValidationState CheckPhone(TextBox txtBPhone)
        {
            ValidationState state = new ValidationState();
            string digitsOnly = new string(txtBPhone.Text.Where(char.IsDigit).ToArray());
            if (!CheckValidation.CheckEmptyNull(txtBPhone.Text))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckPhone(digitsOnly))
                state.SetError("Телефонный номер должен быть указан в формате 10 цифр без кода страны");
            return state;
        }
        public static bool CheckEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        public static ValidationState CheckEmail(TextBox email)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(email.Text))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckEmail(email.Text))
                state.SetError("Некорректный формат email");
            else if (email.Text.Length > 254)
                state.SetError("Email слишком длинный");
            return state;
        }
        public static ValidationState CheckCyrillic(TextBox input)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(input.Text))
                return state;
            if (!Regex.IsMatch(input.Text, @"^[\p{IsCyrillic}-]+$"))
            state.SetError("Разрешены только кириллица и дефис");
            return state;
        }
        public static ValidationState CheckBirthDate(DatePicker birthDate)
        {
            var state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(birthDate.Text))
                return state;
            DateTime minDate = DateTime.Today.AddYears(-120);
            DateTime maxDate = DateTime.Today.AddYears(-14);
            if (birthDate.SelectedDate.Value < minDate)
                state.SetError("Возраст не может быть более 120 лет");
            else if (birthDate.SelectedDate.Value > maxDate)
                state.SetError("Регистрация возможна только с 14 лет");
            return state;
        }
        public static bool CheckEmptyNull(string txt) => !String.IsNullOrEmpty(txt) && !String.IsNullOrWhiteSpace(txt);
        public static bool CheckInt(string txt) => Int32.TryParse(txt.Trim(), out int a);
    }
    public class ValidationState
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Устанавливает состояние ошибки с указанным сообщением.
        /// </summary>
        /// <param name="s">Сообщение об ошибке, которое будет сохранено в свойстве ErrorMessage.
        /// Если передана пустая строка или null, состояние ошибки не изменяется.</param>
        public void SetError(string s)
        {
            ErrorMessage = s;
            IsError = true;
        }
    }
}
