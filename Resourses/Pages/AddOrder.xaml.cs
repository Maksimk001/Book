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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        public AddOrder()
        {
            InitializeComponent();

            ProductsGrid.ItemsSource = AppConnect.model0db.Products.ToList();
            pickup.ItemsSource = AppConnect.model0db.Addresses.ToList();
            user.ItemsSource = AppConnect.model0db.Users.ToList();

            var userr = AppConnect.CurrentUser;
            NameUser.Text = userr.last_name + " " + userr.first_name + " " + userr.middle_name;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAddress = pickup.SelectedItem as Addresses;
                var selectedUser = user.SelectedItem as Users;

                if (selectedAddress == null || selectedUser == null || string.IsNullOrWhiteSpace(code.Text))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }

                var order = new Book.ApplicationData.Orders()
                {
                    date_order = DateTime.Now,
                    date_pickup = deliveryDatePicker.SelectedDate ?? DateTime.Now,

                    address_id = selectedAddress.id,
                    user_id = selectedUser.id,

                    code = Convert.ToInt32(code.Text),
                    status_id = 1
                };

                AppConnect.model0db.Orders.Add(order);
                AppConnect.model0db.SaveChanges();

                var products = ProductsGrid.ItemsSource as List<Products>;

                foreach (var item in products)
                {
                    if (item.Count > 0)
                    {
                        OrderItems oi = new OrderItems()
                        {
                            order_id = order.id,
                            product_id = item.id,
                            quantity = item.Count
                        };

                        AppConnect.model0db.OrderItems.Add(oi);
                    }
                }

                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Заказ создан");
                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            Navigate(new Login(), "Вход");
        }
    }
}
