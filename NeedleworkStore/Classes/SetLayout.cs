using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace NeedleworkStore.Classes
{
    static internal class SetLayout
    {
        public static void SetErrorText(TextBlock textBlock, ValidationState state) => textBlock.Text = state.ErrorMessage;
        public static void ColorInputControl(Control control, bool isError)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            var brush = isError
                ? new SolidColorBrush(Color.FromArgb(100, 251, 174, 210))
                : Brushes.White;
            control.Background = brush;
        }
    }
}
