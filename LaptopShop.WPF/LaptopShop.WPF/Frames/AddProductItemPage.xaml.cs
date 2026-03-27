using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class AddProductItemPage : Page
    {
        private int _warehouseId;
        private ProductRepository _productRepo = new ProductRepository();
        private ProductItemRepository _itemRepo = new ProductItemRepository();

        public AddProductItemPage(int warehouseId)
        {
            InitializeComponent();
            _warehouseId = warehouseId;
            LoadProducts();
        }

        private void LoadProducts()
        {
            cbProducts.ItemsSource = _productRepo.GetAll();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                if (cbProducts.SelectedValue == null || string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm và nhập số lượng!");
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương!");
                    return;
                }

                // 2. Lấy thông tin
                int productId = (int)cbProducts.SelectedValue;
                string prefix = txtSerialPrefix.Text.Trim();

                // Lấy trạng thái từ ComboBox chọn
                string status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "InStock";

                List<ProductItem> newItems = new List<ProductItem>();
                string timestamp = DateTime.Now.ToString("yyMMddHHmm");

                for (int i = 1; i <= quantity; i++)
                {
                    newItems.Add(new ProductItem
                    {
                        ProductId = productId,
                        WarehouseId = _warehouseId,
                        // Nếu nhân viên nhập hàng lỗi, khuyên họ đổi Prefix thành ERR- hoặc vỡ-
                        SerialNumber = $"{prefix}{timestamp}-{i:D3}",
                        Status = status
                    });
                }

                // 3. Lưu vào DB
                _itemRepo.AddRange(newItems);

                MessageBox.Show($"Thành công! Đã thêm {quantity} sản phẩm trạng thái {status}. \nBạn có thể tiếp tục nhập lô hàng khác hoặc nhấn Quay lại.");

                // 4. RESET để nhập lần 2 (Không dùng GoBack ở đây)
                txtQuantity.Clear();
                txtSerialPrefix.Clear();
                txtQuantity.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Nút này bây giờ đóng vai trò là "Hoàn tất công việc và quay ra"
            this.NavigationService.GoBack();
        }
    }
}