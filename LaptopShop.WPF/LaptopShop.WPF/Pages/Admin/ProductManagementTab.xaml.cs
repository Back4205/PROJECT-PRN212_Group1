using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Pages.Admin
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
                // 1. Lấy dữ liệu và Trim khoảng trắng
                string code = txtProductCode.Text.Trim();
                string name = txtProductName.Text.Trim();
                string brand = txtBrand.Text.Trim();
                string priceText = txtBasePrice.Text.Trim();

                // 2. Kiểm tra bỏ trống (Add thiếu code, name...)
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(brand))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ: Mã, Tên và Thương hiệu sản phẩm!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 3. Kiểm tra định dạng giá và giá > 0
                if (!decimal.TryParse(priceText, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Giá sản phẩm phải là số và lớn hơn 0!", "Lỗi định dạng giá", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 4. Kiểm tra trùng Mã sản phẩm (Cực kỳ quan trọng)
                // Giả sử má đã có hàm GetByCode trong Service
                var existing = _productService.GetAll().FirstOrDefault(p => p.ProductCode.Equals(code, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    MessageBox.Show($"Mã sản phẩm '{code}' đã tồn tại trong hệ thống rồi má ơi!", "Trùng mã", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 5. Nếu vượt qua hết thì mới tạo Object và gọi Add
                var newProduct = new Product
                {
                    ProductCode = code,
                    ProductName = name,
                    Brand = brand,
                    BasePrice = price,
                    ImgUrl = txtImgUrl.Text,
                    IsActive = chkIsActive.IsChecked ?? true
                };

                _productService.Add(newProduct);
                MessageBox.Show("Thêm sản phẩm thành công rực rỡ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadProducts(); // Load lại bảng
                ClearForm();    // Xóa trắng ô nhập
            }
            catch (Exception ex)
            {
                // Đây là "lưới bảo hiểm" cuối cùng nếu có lỗi lạ
                MessageBox.Show("Lỗi phát sinh: " + ex.Message);
            }
        }
        private void ClearForm()
        {
            txtProductId.Text = "";
            txtProductCode.Text = "";
            txtProductName.Text = "";
            txtBrand.Text = "";
            txtBasePrice.Text = "";
            txtImgUrl.Text = "";
            chkIsActive.IsChecked = true;
        }
        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Lấy ID từ TextBox (để biết đang update thằng nào)
                if (!int.TryParse(txtProductId.Text, out int id)) return;

                // 2. Tạo object mang thông tin MỚI từ các ô nhập liệu
                var updatedProduct = new Product
                {
                    ProductId = id,
                    ProductCode = txtProductCode.Text,
                    ProductName = txtProductName.Text,
                    Brand = txtBrand.Text,
                    BasePrice = decimal.Parse(txtBasePrice.Text),
                    ImgUrl = txtImgUrl.Text,

                    // DÒNG QUAN TRỌNG NHẤT: Má phải gán giá trị từ CheckBox vào đây!
                    IsActive = chkIsActive.IsChecked ?? false
                };

                // 3. Gọi Service để lưu vào DB
                _productService.Update(updatedProduct);

                MessageBox.Show("Cập nhật thành công!");

                // 4. Load lại bảng để thấy thay đổi
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi update: " + ex.Message);
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