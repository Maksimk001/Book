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
    /// Логика взаимодействия для CatalogAdmin.xaml
    /// </summary>
    public partial class CatalogAdmin : Page
    {
        public CatalogAdmin()
        {
            InitializeComponent();
        }
        List<Products> allProducts;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            allProducts = AppConnect.model0db.Products.ToList();
            CatalogDataGrid.ItemsSource = allProducts;

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
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить товар?", "Подтвердите выбор", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var currentProduct = CatalogDataGrid.SelectedItem as Products;
                    var hasOrders = AppConnect.model0db.OrderItems.Any(x => x.id == currentProduct.id);

                    if (hasOrders)
                    {
                        MessageBox.Show("Нельзя удалить товар, так как он есть в заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    AppConnect.model0db.Products.Remove(currentProduct);

                    AppConnect.model0db.SaveChanges();
                    CatalogDataGrid.ItemsSource = AppConnect.model0db.Products.ToList();
                    MessageBox.Show("Товар удален", "Успех!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CatalogDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var product = CatalogDataGrid.SelectedItem as Products;

            if (product != null)
            {
                Navigate(new EditProduct(product), "Редактировать товар");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new AddProduct(), "Добавить товар");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new OrdersAdmin(), "Заказы");
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            AppConnect.CurrentUser = null;
            Navigate(new Login(), "Вход");
        }
    }
}
