using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace NeedleworkStore.Classes
{
    static internal class CheckValidation
    {      
        public static bool CheckLogin(string txt)
        {
            foreach (char ch in txt)
            {
                if (ch == '&' || ch == '+' || ch == '=' || ch == '<' || ch == '>' || ch == ',' || ch == '-' || ch == '"' || ch == '\'')
                {
                    return false;
                }
            }
            return true;
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
            if (txt.Length != 12 && txt.Length != 11)
                return false;
            if (txt.StartsWith("+7") && txt.Length != 12)
                return false;
            if (txt.StartsWith("8") && txt.Length != 11)
                return false;
            for (int i = 1; i < txt.Length; i++)
            {
                if (!char.IsDigit(txt[i]))
                    return false;
            }
            return true;
        }
        public static string CorrectPhone(string txt)
        {
            if (txt.StartsWith("+7") && txt.Length == 12)
                return txt.Substring(2);
            if (txt.StartsWith("8") && txt.Length == 11)
                return txt.Substring(1);
            return txt;
        }
        public static ValidationState CheckPhone(TextBox txtBPhone)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(txtBPhone.Text))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckPhone(txtBPhone.Text))
                state.SetError("Телефон должен начинаться с +7 или 8 и содержать 11 цифр");
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
