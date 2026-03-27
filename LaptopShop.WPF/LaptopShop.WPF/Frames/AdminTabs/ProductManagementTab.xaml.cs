using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames.AdminTabs
{
    public partial class ProductManagementTab : UserControl
    {
        private readonly IProductService _productService;

        public event RoutedEventHandler? DataChanged;

        public ProductManagementTab()
        {
            InitializeComponent();
            _productService = new ProductService();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                dgProducts.ItemsSource = null;
                dgProducts.ItemsSource = _productService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dgProducts.ItemsSource = null;
                dgProducts.ItemsSource = _productService.Search(txtSearchProduct.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                txtProductId.Text = product.ProductId.ToString();
                txtProductCode.Text = product.ProductCode;
                txtProductName.Text = product.ProductName;
                txtBrand.Text = product.Brand;
                txtBasePrice.Text = product.BasePrice.ToString();
                txtImgUrl.Text = product.ImgUrl;
                chkIsActive.IsChecked = product.IsActive;
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtBasePrice.Text, out decimal price))
                {
                    MessageBox.Show("Price phải là số hợp lệ.");
                    return;
                }

                Product product = new Product
                {
                    ProductCode = txtProductCode.Text,
                    ProductName = txtProductName.Text,
                    Brand = txtBrand.Text,
                    BasePrice = price,
                    ImgUrl = txtImgUrl.Text,
                    IsActive = chkIsActive.IsChecked ?? true
                };

                _productService.Add(product);
                MessageBox.Show("Thêm sản phẩm thành công.");
                LoadProducts();
                ClearProductForm();
                DataChanged?.Invoke(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductId.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm.");
                    return;
                }

                if (!decimal.TryParse(txtBasePrice.Text, out decimal price))
                {
                    MessageBox.Show("Price phải là số hợp lệ.");
                    return;
                }

                Product product = new Product
                {
                    ProductId = int.Parse(txtProductId.Text),
                    ProductCode = txtProductCode.Text,
                    ProductName = txtProductName.Text,
                    Brand = txtBrand.Text,
                    BasePrice = price,
                    ImgUrl = txtImgUrl.Text,
                    IsActive = chkIsActive.IsChecked ?? true
                };

                _productService.Update(product);
                MessageBox.Show("Cập nhật sản phẩm thành công.");
                LoadProducts();
                ClearProductForm();
                DataChanged?.Invoke(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductId.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm.");
                    return;
                }

                int id = int.Parse(txtProductId.Text);
                _productService.Delete(id);

                MessageBox.Show("Xử lý xóa sản phẩm thành công.");
                LoadProducts();
                ClearProductForm();
                DataChanged?.Invoke(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearProductForm_Click(object sender, RoutedEventArgs e)
        {
            ClearProductForm();
        }

        private void ClearProductForm()
        {
            txtProductId.Text = "";
            txtProductCode.Text = "";
            txtProductName.Text = "";
            txtBrand.Text = "";
            txtBasePrice.Text = "";
            txtImgUrl.Text = "";
            chkIsActive.IsChecked = true;
            dgProducts.SelectedItem = null;
        }
    }
}