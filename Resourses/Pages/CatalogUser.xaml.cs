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
    /// Логика взаимодействия для CatalogUser.xaml
    /// </summary>
    public partial class CatalogUser : Page
    {
        public CatalogUser()
        {
            InitializeComponent();

            CatalogDataGrid.ItemsSource = AppConnect.model0db.Products.ToList();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            Navigate(new Login(), "Вход");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var user = AppConnect.CurrentUser;
            NameUser.Text = user.last_name + " " + user.first_name + " " + user.middle_name;
        }
    }
}
