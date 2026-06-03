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
using static Book.MainWindow;

namespace Book.Resourses.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (loginI.Text == null || passwordI.Text == null)
                {
                    MessageBox.Show("Заполните все поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                var userObj = AppConnect.model0db.Users.FirstOrDefault(x => x.login == loginI.Text && x.password == passwordI.Text);
                if (userObj == null)
                {
                    MessageBox.Show("Пользователь не найден", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (userObj.role_id == 3)
                    {
                        Navigate(new CatalogAdmin(), "Каталог");
                    }
                    else if (userObj.role_id == 2)
                    {
                        Navigate(new CatalogManager(), "Каталог");
                    }
                    else if (userObj.role_id == 1)
                    {
                        Navigate(new CatalogUser(), "Каталог");
                    }
                    AppConnect.CurrentUser = userObj;
                    MessageBox.Show("Добро пожаловать, " + userObj.first_name, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Уведомление", MessageBoxButton.OK);
            }
        }

        private void GuestLogin_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new CatalogGuest(), "Каталог");
            MessageBox.Show("Добро пожаловать " + "Гость", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
