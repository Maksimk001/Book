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
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Page
    {
        Products currentProduct;
        string selectedImageName;
        public EditProduct(Products product)
        {
            InitializeComponent();

            currentProduct = product;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CmbEd.ItemsSource = AppConnect.model0db.Units.ToList();
            CmbProviders.ItemsSource = AppConnect.model0db.Providers.ToList();
            CmdManuf.ItemsSource = AppConnect.model0db.Manufacturers.ToList();
            CmdCategory.ItemsSource = AppConnect.model0db.Categories.ToList();

            if (currentProduct != null)
            {
                art.Text = currentProduct.article;
                name.Text = currentProduct.name;
                price.Text = currentProduct.price.ToString();
                sale.Text = currentProduct.sale.ToString();
                quantity.Text = currentProduct.quantity.ToString();
                description.Text = currentProduct.description;

                string imagePath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Images",
                    "ProductImage",
                    currentProduct.image_path ?? "picture.png"
                );

                if (!System.IO.File.Exists(imagePath))
                {
                    imagePath = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images",
                        "ProductImage",
                        "picture.png"
                    );
                }

                ProductImage.Source = new BitmapImage(new Uri(imagePath));

                CmbEd.SelectedItem = CmbEd.Items.Cast<Units>().FirstOrDefault(x => x.id == currentProduct.unit_id);

                CmbProviders.SelectedItem = CmbProviders.Items.Cast<Providers>().FirstOrDefault(x => x.id == currentProduct.provider_id);

                CmdManuf.SelectedItem = CmdManuf.Items.Cast<Manufacturers>().FirstOrDefault(x => x.id == currentProduct.manufacturer_id);

                CmdCategory.SelectedItem = CmdCategory.Items.Cast<Categories>().FirstOrDefault(x => x.id == currentProduct.category_id);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                if (currentProduct == null)
                {
                    currentProduct = new Products();
                    AppConnect.model0db.Products.Add(currentProduct);
                }

                currentProduct.article = art.Text;
                currentProduct.name = name.Text;
                currentProduct.price = Convert.ToDecimal(price.Text);
                currentProduct.sale = Convert.ToInt32(sale.Text);
                currentProduct.quantity = Convert.ToInt32(quantity.Text);
                currentProduct.description = description.Text;

                currentProduct.unit_id = (CmbEd.SelectedItem as Units).id;
                currentProduct.provider_id = (CmbProviders.SelectedItem as Providers).id;
                currentProduct.manufacturer_id = (CmdManuf.SelectedItem as Manufacturers).id;
                currentProduct.category_id = (CmdCategory.SelectedItem as Categories).id;

                if (!string.IsNullOrWhiteSpace(selectedImageName))
                {
                    currentProduct.image_path = selectedImageName;
                }
                else if (string.IsNullOrWhiteSpace(currentProduct.image_path))
                {
                    currentProduct.image_path = "picture.png";
                }

                AppConnect.model0db.SaveChanges();

                MessageBox.Show("Сохранено");
                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image (*.jpg;*.png)|*.jpg;*.png"
                };

                if (dialog.ShowDialog() == true)
                {
                    string folder = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images",
                        "ProductImage");

                    if (!System.IO.Directory.Exists(folder))
                    {
                        System.IO.Directory.CreateDirectory(folder);
                    }

                    string fileName = System.IO.Path.GetFileName(dialog.FileName);

                    string fullPath = System.IO.Path.Combine(folder, fileName);

                    System.IO.File.Copy(dialog.FileName, fullPath, true);

                    selectedImageName = fileName;

                    ProductImage.Source = new BitmapImage(new Uri(fullPath));
                }
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
    }
}
