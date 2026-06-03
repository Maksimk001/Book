using Book.ApplicationData;
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
using static Book.ApplicationData.AppFrame;

namespace Book.Resourses.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersAdmin.xaml
    /// </summary>
    public partial class OrdersAdmin : Page
    {
        public OrdersAdmin()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OrdersGrid.ItemsSource = AppConnect.model0db.Orders.ToList();

            var user = AppConnect.CurrentUser;
            NameUser.Text = user.last_name + " " + user.first_name + " " + user.middle_name;
        }

        private void OrdersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedOrder = OrdersGrid.SelectedItem as Book.ApplicationData.Orders;

            if (selectedOrder != null)
            {
                Navigate(new EditOrder(selectedOrder), "Редактирование заказа");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new AddOrder(), "Редактирование заказа");
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedOrder = OrdersGrid.SelectedItem as Book.ApplicationData.Orders;

                if (selectedOrder == null)
                {
                    MessageBox.Show("Выберите заказ");
                    return;
                }

                var result = MessageBox.Show("Удалить заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                    return;

                var orderItems = AppConnect.model0db.OrderItems.Where(x => x.order_id == selectedOrder.id).ToList();

                AppConnect.model0db.OrderItems.RemoveRange(orderItems);

                AppConnect.model0db.Orders.Remove(selectedOrder);

                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Удалено");

                OrdersGrid.ItemsSource = AppConnect.model0db.Orders.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            Navigate(new Login(), "Вход");
        }
    }
}
