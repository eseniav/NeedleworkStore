using NeedleworkStore.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NeedleworkStore.Classes;
using System.Data.Entity;

namespace NeedleworkStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderRegistrationPage.xaml
    /// </summary>
    public partial class OrderRegistrationPage : Page
    {
        List<Carts> orderCart;
        List<Cities> city;
        List<PickUpPoints> pickUpPoints;
        Cities selectedCity;
        PickUpPoints selectedPickUpPoint;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public OrderRegistrationPage(List<Carts> crt)
        {
            InitializeComponent();
            orderCart = crt;
            city = App.ctx.Cities.ToList();
            pickUpPoints = App.ctx.PickUpPoints.ToList();
            cmbPickUpPointCity.ItemsSource = city;
            cmbPickUpPointAddress.ItemsSource = pickUpPoints;
            selectedCity = null;
            selectedPickUpPoint = null;
            ICOrderReg.ItemsSource = orderCart;
            ICOrderReg.DataContext = orderCart;
            ChangeSelectedQuantityBottomMenu();
            SetTotalSum();
            cmbPayAfter.IsSelected = true;
        }
        private void ChangeSelectedQuantityBottomMenu()
        {
            lblTotalQuantity.Content = orderCart.Sum(p => p.QuantityCart).ToString();
        }
        private void SetTotalSum() => lblTotalSum.Content = orderCart.Sum(c => c.SumProducts).ToString();
        private void MinusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity--;
            try
            {
                App.ctx.SaveChanges();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка базы данных");
            }
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            MinusQuantity((Carts)((RepeatButton)sender).DataContext, (RepeatButton)sender);
        }
        private void PlusQuantity(Carts cr, RepeatButton rb)
        {
            cr.Quantity++;
            try
            {
                App.ctx.SaveChanges();
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            PlusQuantity((Carts)((RepeatButton)sender).DataContext, (RepeatButton)sender);
        }
        private void DelOneProduct(Carts cr)
        {
            MessageBoxResult msgInf = MessageBox.Show
                    ("Этот товар будет удалён из заказа",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (msgInf != MessageBoxResult.Yes)
                return;
            try
            {
                orderCart.Remove(cr);
                if(orderCart.Count == 0)
                {
                    this.NavigationService.Navigate(new CartPage());
                    return;
                }
                SetTotalSum();
                ChangeSelectedQuantityBottomMenu();
                ICOrderReg.ItemsSource = orderCart;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            DelOneProduct((Carts)((Button)sender).DataContext);
        }
        private bool CheckConditions(string card)
        {
            if (cmbPickUpPointCity.SelectedItem == null || cmbPickUpPointAddress.SelectedItem == null || cmbPayment.SelectedItem == null)
                return false;
            if (cmbPayNow.IsSelected == true)
                return CheckCard.CheckCardNumber(card) && CheckCard.IsNotExpired(txtBMonth.Text, txtBYear.Text);
            return true;
        }
        private void btnPOrderReg_Click(object sender, RoutedEventArgs e)
        {
            string card = txtBCardNum.Text.Replace(" ", "");
            if (!CheckConditions(card))
            {
                string errorMessage = "Не удалось оформить заказ.\n";
                if (cmbPickUpPointCity.SelectedItem == null)
                    errorMessage += "• Не выбран город\n";
                if (cmbPickUpPointAddress.SelectedItem == null)
                    errorMessage += "• Не выбран пункт выдачи\n";
                if (cmbPayment.SelectedItem == null)
                    errorMessage += "• Не выбран способ оплаты\n";
                if (cmbPayNow.IsSelected == true && !CheckCard.ContainsOnlyDigits(card))
                    errorMessage += "• Номер карты должен содержать только цифры\n";

                MessageBox.Show(
                    errorMessage + "\nПожалуйста, заполните все поля правильно",
                    "Проверьте данные",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
             try
            {
                int paymentID = cmbPayNow.IsSelected ? 1 : 2;
                Orders newOrder = new Orders
                {
                    UserID = (int)mainWindow.UserID,
                    PickUpPointID = selectedPickUpPoint.PickUpPointID,
                    FormationDate = DateTime.Now,
                    CardNumber = card,
                };
                App.ctx.Orders.Add(newOrder);
                foreach (Carts list in orderCart)
                {
                    OrderCompositions newOrderComposition = new OrderCompositions
                    {
                        Quantity = list.QuantityCart,
                        ProductID = list.ProductID,
                        OrderID = newOrder.OrderID,
                        OrderPrice = (int)list.SumProducts,
                    };
                    App.ctx.OrderCompositions.Add(newOrderComposition);
                }
                AssigningStatuses newAssigningStatuses = new AssigningStatuses
                {
                    PaymentStatusID = paymentID,
                    ReceivingStatusID = 2,
                    ModifiedDate = DateTime.Now,
                    OrderID = newOrder.OrderID,
                    ProcessingStatusID = 1,
                };
                App.ctx.AssigningStatuses.Add(newAssigningStatuses);
                foreach (Carts list in orderCart)
                {
                    App.ctx.Carts.Remove(list);
                }
                App.ctx.SaveChanges();
                MessageBox.Show(
                            "✅ Ваш заказ успешно оформлен!\n\n" +
                            $"Номер заказа: #{newOrder.OrderID}\n" +
                            "После оплаты чек в PDF-формате доступен для скачивания:\n" +
                            "1. Откройте «Профиль»\n" +
                            "2. Перейдите в раздел «Заказы»\n" +
                            "3. Найдите текущий заказ\n" +
                            "4. Нажмите кнопку «Скачать чек»\n\n" +
                            "Спасибо за покупку!",
                            "Заказ подтверждён",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                this.NavigationService.Navigate(new OrdersPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SetPickUpPointAddress(string address)
        {
            selectedPickUpPoint = App.ctx.PickUpPoints.FirstOrDefault(p => p.Adress == address);
            if (selectedCity == null)
            {
                selectedCity = App.ctx.Cities.FirstOrDefault(c => c.CityID == selectedPickUpPoint.CityID);
                cmbPickUpPointCity.SelectedItem = selectedCity;
            }
        }
        private void cmbPickUpPointAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointAddress(((PickUpPoints)((ComboBox)sender).SelectedItem).Adress);
        }
        private void FilterAddressList()
        {
            pickUpPoints = App.ctx.PickUpPoints.ToList();
            cmbPickUpPointAddress.SelectedItem = App.ctx.PickUpPoints.FirstOrDefault(c => c.CityID == selectedCity.CityID);
            pickUpPoints = App.ctx.PickUpPoints.Where(c => c.CityID == selectedCity.CityID).ToList();
            cmbPickUpPointAddress.ItemsSource = pickUpPoints;
        }
        private void SetVisibilityPayment(string cmbName)
        {
            if (cmbName == "cmbPayAfter")
            {
                foreach (UIElement child in grdBottomInterface.Children)
                {
                    if (Grid.GetColumn(child) == 1)
                        child.Visibility = Visibility.Hidden;
                }
            } else
            {
                foreach (UIElement child in grdBottomInterface.Children)
                {
                    if (Grid.GetColumn(child) == 1)
                        child.Visibility = Visibility.Visible;
                }
            }
        }
        private void cmbPayment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetVisibilityPayment(((ComboBoxItem)((ComboBox)sender).SelectedItem).Name);
        }
        private void SetPickUpPointCity(string cityName)
        {
            selectedCity = App.ctx.Cities.FirstOrDefault(c => c.CityName == cityName);
            FilterAddressList();
        }
        private void cmbPickUpPointCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPickUpPointCity(((Cities)((ComboBox)sender).SelectedItem).CityName);
        }

        private void txtBCardNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            //SetLayout.ColorInputControl((TextBox)sender, !CheckCard.CheckCardNumber(((TextBox)sender).Text));
            var textBox = sender as TextBox;
            if (textBox == null) return;
            string text = textBox.Text.Replace(" ", "");
            if (text.Length > 16)
            {
                text = text.Substring(0, 16);
            }
            StringBuilder formattedText = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedText.Append(" ");
                }
                formattedText.Append(text[i]);
            }
            if (textBox.Text != formattedText.ToString())
            {
                int caretPos = textBox.CaretIndex;
                textBox.Text = formattedText.ToString();
                textBox.CaretIndex = caretPos + (formattedText.Length - text.Length);
            }
            string cleanCardNumber = formattedText.ToString().Replace(" ", "");
            SetLayout.ColorInputControl(textBox, !CheckCard.CheckCardNumber(cleanCardNumber));
        }
        private void txtBMonth_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetLayout.ColorInputControl((TextBox)sender, !CheckCard.IsValidMonth(((TextBox)sender).Text));
            if (((TextBox)sender).Text.Length == 2 && CheckCard.IsValidMonth(((TextBox)sender).Text))
                txtBYear.Focus();
        }
        private void txtBYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetLayout.ColorInputControl((TextBox)sender, !CheckCard.IsValidYear(((TextBox)sender).Text));
        }
    }
}
