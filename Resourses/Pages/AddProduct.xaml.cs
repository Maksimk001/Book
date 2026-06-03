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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        public AddProduct()
        {
            InitializeComponent();

            CmbEd.ItemsSource = AppConnect.model0db.Units.ToList();
            CmbProviders.ItemsSource = AppConnect.model0db.Providers.ToList();
            CmdManuf.ItemsSource = AppConnect.model0db.Manufacturers.ToList();
            CmdCategory.ItemsSource = AppConnect.model0db.Categories.ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Products product = new Products();

                if (art.Text == "" || name.Text == "" || price.Text == "" || sale.Text == "" || quantity.Text == "" || description.Text == "")
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Convert.ToInt32(price.Text) < 1 || Convert.ToInt32(quantity.Text) < 1)
                {
                    MessageBox.Show("Цена и количество не может быть меньше 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                product.article = art.Text;
                product.name = name.Text;
                product.price = Convert.ToInt32(price.Text);
                product.sale = Convert.ToInt32(sale.Text);
                product.quantity = Convert.ToInt32(quantity.Text);
                product.description = description.Text;

                product.image_path = selectedImageName ?? "picture.png";

                var CurrentEd = CmbEd.SelectedItem as Units;
                if (CurrentEd == null)
                {
                    MessageBox.Show("Выберите еденицу измерения", "Ошибка");
                    return;
                }

                var CurrentProvider = CmbProviders.SelectedItem as Providers;
                if (CurrentProvider == null)
                {
                    MessageBox.Show("Выберите поставщика", "Ошибка");
                    return;
                }

                var CurrentManufacturer = CmdManuf.SelectedItem as Manufacturers;
                if (CurrentManufacturer == null)
                {
                    MessageBox.Show("Выберите поставщика", "Ошибка");
                    return;
                }

                var CurrentCategory = CmdCategory.SelectedItem as Categories;
                if (CurrentCategory == null)
                {
                    MessageBox.Show("Выберите категорию", "Ошибка");
                    return;
                }

                product.unit_id = CurrentEd.id;
                product.provider_id = CurrentProvider.id;
                product.manufacturer_id= CurrentManufacturer.id;
                product.category_id = CurrentCategory.id;

                AppConnect.model0db.Products.Add(product);
                AppConnect.model0db.SaveChanges();
                MessageBox.Show("Товар успешно добавлен", "Успех");
                GoBack();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Ошибка");
            }
        }

        string selectedImageName = null;

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";

                if (openFile.ShowDialog() == true)
                {
                    ProductImage.Source = new BitmapImage(new Uri(openFile.FileName));

                    selectedImageName = System.IO.Path.GetFileName(openFile.FileName);

                    string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "ProductImage");

                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }

                    string fullPath = System.IO.Path.Combine(folderPath, selectedImageName);

                    System.IO.File.Copy(openFile.FileName, fullPath, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
    }
}
