using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
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

namespace LaptopShop.WPF.Pages.Customer
{
    public partial class ProductPage : Page
    {
        private readonly ProductService _productService;
        private readonly User _user;

        public ProductPage(User user)
        {
            InitializeComponent();
            _productService = new ProductService();
            _user = user;
            BrandFilterBox.SelectedIndex = 0;
            PriceFilterBox.SelectedIndex = 0;
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductList.ItemsSource = _productService.GetAll();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        // Triggered whenever the Brand or Price dropdown changes
        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Auto-search when selection changes
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            // Kiểm tra an toàn để tránh lỗi NullReference khi Page mới load
            if (SearchBox == null || BrandFilterBox == null || PriceFilterBox == null) return;

            string keyword = SearchBox.Text;

            // Lấy Brand từ Tag của ComboBoxItem
            string brand = (BrandFilterBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (brand == "") brand = null; // Nếu chọn "All Brands" thì để null

            decimal minPrice = 0;
            decimal maxPrice = decimal.MaxValue;

            // Tách chuỗi "min-max" từ Tag của PriceFilterBox
            if (PriceFilterBox.SelectedItem is ComboBoxItem selectedPrice)
            {
                string range = selectedPrice.Tag.ToString();
                var parts = range.Split('-');
                if (parts.Length == 2)
                {
                    decimal.TryParse(parts[0], out minPrice);
                    decimal.TryParse(parts[1], out maxPrice);
                }
            }

            // Gọi đúng 4 tham số như Service đã định nghĩa
            var result = _productService.SearchAndFilter(keyword, brand, minPrice, maxPrice);
            ProductList.ItemsSource = result;
        }
    }
}