using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        public  static ValidationState CheckPassword(PasswordBox psxB)
        {
            ValidationState state = new ValidationState();
            if (!CheckValidation.CheckEmptyNull(psxB.Password))
                state.SetError("Поле обязательно для заполнения");
            else if (!CheckValidation.CheckPass(psxB.Password))
                state.SetError("Поле заполнено неправильно");
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
