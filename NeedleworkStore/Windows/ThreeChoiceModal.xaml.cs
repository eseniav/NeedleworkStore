using NeedleworkStore.Pages;
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
using System.Windows.Shapes;

namespace NeedleworkStore.Windows
{
    /// <summary>
    /// Interaction logic for ThreeChoiceModal.xaml
    /// </summary>
    public partial class ThreeChoiceModal : Window
    {
        public int? Choice { get; private set; }
        public string Message { get; set; } = "Message not set";
        public string CancelText { get; set; } = "Cancel";
        public string Opt1Text { get; set; } = "Option 1";
        public string Opt2Text { get; set; } = "Option 2";
        public string WindowTitle { get; set; } = "Title unset";
        public ThreeChoiceModal()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public ThreeChoiceModal(string msg, string title, string opt1 = "Option1", string opt2 = "Option2", string cancel = "Cancel") : this()
        {
            this.Title = title;
            this.MsgTxb.Text = msg;
            Opt1Btn.Content = opt1;
            Opt2Btn.Content = opt2;
            CancelBtn.Content = cancel;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Choice = 0;
        }
        private void Option1_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Choice = 1;
        }
        private void Option2_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Choice = 2;
        }
    }
}
