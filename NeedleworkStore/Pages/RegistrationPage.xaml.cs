﻿using System;
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
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void btnAuthReg_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("Pages/AuthPage.xaml", UriKind.Relative));
            this.NavigationService.Navigate(new AuthPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            } 
            else
            {
                MessageBox.Show("No page to go back");
            }            
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }
            else
            {
                MessageBox.Show("No page to go forward");
            }
        }
    }
}
