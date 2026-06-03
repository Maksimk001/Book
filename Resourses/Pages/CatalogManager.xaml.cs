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
    /// Логика взаимодействия для CatalogManager.xaml
    /// </summary>
    public partial class CatalogManager : Page
    {
        public List<Products> Products { get; set; }
        public CatalogManager()
        {
            InitializeComponent();
        }
        public List<Products> allProducts { get; set; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            allProducts = AppConnect.model0db.Products.ToList();
            CatalogDataGrid.ItemsSource = allProducts;

            var providers = AppConnect.model0db.Providers.ToList();
            providers.Insert(0, new Providers { name = "Все поставщики" });

            SortBox.SelectedIndex = 0;
            DiscountFilter.SelectedIndex = 0;

            var user = AppConnect.CurrentUser;
            NameUser.Text = user.last_name + " " + user.first_name + " " + user.middle_name;
        }
        private void Filter_Changed(object sender, EventArgs e)
        {
            var result = allProducts.ToList();

            // Поиск
            string text = SearchBox.Text?.ToLower();

            if (!string.IsNullOrWhiteSpace(text))
            {
                result = result.Where(p =>
                    p.name.ToLower().Contains(text) ||
                    p.description.ToLower().Contains(text) ||
                    p.article.ToLower().Contains(text)
                ).ToList();
            }

            // Фильтр по скидке
            switch (DiscountFilter.SelectedIndex)
            {
                case 1:
                    result = result.Where(p => p.sale >= 0 && p.sale <= 12.99).ToList();
                    break;

                case 2:
                    result = result.Where(p => p.sale >= 13 && p.sale <= 16.99).ToList();
                    break;

                case 3:
                    result = result.Where(p => p.sale >= 17).ToList();
                    break;
            }

            switch (SortBox.SelectedIndex)
            {
                case 1:
                    result = result.OrderBy(p => p.price).ToList();
                    break;

                case 2:
                    result = result.OrderByDescending(p => p.price).ToList();
                    break;

                case 3:
                    result = result.OrderBy(p => p.quantity).ToList();
                    break;

                case 4:
                    result = result.OrderByDescending(p => p.quantity).ToList();
                    break;
            }

            CatalogDataGrid.ItemsSource = result;
        }


        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            Navigate(new Login(), "Вход");
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new OrdersManager(), "Заказы");
        }
    }
}
