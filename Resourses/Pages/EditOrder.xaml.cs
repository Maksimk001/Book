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
    /// Логика взаимодействия для EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Page
    {
        private Book.ApplicationData.Orders currentOrder;

        public EditOrder(Orders order)
        {
            InitializeComponent();

            currentOrder = order;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pickup.ItemsSource = AppConnect.model0db.Addresses.ToList();
            user.ItemsSource = AppConnect.model0db.Users.ToList();

            pickup.SelectedValue = currentOrder.address_id;
            user.SelectedValue = currentOrder.user_id;

            deliveryDatePicker.SelectedDate = currentOrder.date_pickup;
            code.Text = Convert.ToString(currentOrder.code);
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

                currentOrder.address_id = selectedAddress.id;
                currentOrder.user_id = selectedUser.id;
                currentOrder.code = Convert.ToInt32(code.Text);
                currentOrder.date_order = deliveryDatePicker.SelectedDate ?? DateTime.Now;

                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Заказ обновлён");
                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
    }
}
