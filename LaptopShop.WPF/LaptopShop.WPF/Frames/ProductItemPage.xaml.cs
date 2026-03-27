using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class ProductItemPage : Page
    {
        private readonly int _warehouseId;
        private readonly IProductItemRepository _productItemRepo;
        private readonly IProductRepository _productRepo;
        private List<ProductItem> _fullList = new List<ProductItem>();
        private ProductItem _selectedItemForEdit;

        public ProductItemPage(int warehouseId)
        {
            InitializeComponent();
            _warehouseId = warehouseId;
            _productItemRepo = new ProductItemRepository();
            _productRepo = new ProductRepository();
            LoadData();
        }

        private void LoadData()
        {
            // Khởi tạo lại Repo để xóa sạch Context cũ
            IProductItemRepository freshRepo = new ProductItemRepository();

            _fullList = freshRepo.GetByWarehouseId(_warehouseId);

            dgProductItems.ItemsSource = null;
            dgProductItems.ItemsSource = _fullList;
            txtHeader.Text = $"KHO #{_warehouseId} ({_fullList.Count} thiết bị)";

            // CHÈN MỚI: Chỉ lấy các sản phẩm có IsActive = true để nhập kho
            var activeProducts = _productRepo.GetAll()
                                             .Where(p => p.IsActive == true)
                                             .ToList();

            cbProducts.ItemsSource = activeProducts;
            cbEditProduct.ItemsSource = activeProducts;
        }

        // ================= XỬ LÝ NHẬP KHO (ADD) =================
        private void btnOpenAddForm_Click(object sender, RoutedEventArgs e) => gridAddForm.Visibility = Visibility.Visible;
        private void btnCancel_Click(object sender, RoutedEventArgs e) => gridAddForm.Visibility = Visibility.Collapsed;

        private void btnConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbProducts.SelectedValue == null || string.IsNullOrWhiteSpace(txtSerialList.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm và nhập danh sách Serial!");
                return;
            }

            int pId = (int)cbProducts.SelectedValue;

            // CHÈN MỚI: Lấy trạng thái từ ComboBox cbAddStatus (Logic bạn muốn thêm)
            string initialStatus = (cbAddStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "InStock";

            // GIỮ NGUYÊN LOGIC CŨ: Tách danh sách Serial theo từng dòng
            var serialLines = txtSerialList.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(s => s.Trim())
                                                .Distinct()
                                                .ToList();

            List<ProductItem> validItems = new List<ProductItem>();
            List<string> duplicateSerials = new List<string>();

            foreach (var serial in serialLines)
            {
                // Kiểm tra trùng trong Database
                if (_productItemRepo.IsSerialExists(serial))
                {
                    duplicateSerials.Add(serial);
                }
                else
                {
                    validItems.Add(new ProductItem
                    {
                        ProductId = pId,
                        SerialNumber = serial,
                        WarehouseId = _warehouseId,
                        Status = initialStatus // Dùng biến mới thay vì fix cứng "InStock"
                    });
                }
            }

            // Nếu có mã trùng, thông báo cho người dùng và không cho nhập gì cả
            if (duplicateSerials.Any())
            {
                string errorMsg = "Các số Serial sau đã tồn tại trong hệ thống:\n" + string.Join(", ", duplicateSerials);
                MessageBox.Show(errorMsg, "Lỗi trùng dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Chỉ lưu khi toàn bộ danh sách hợp lệ
                _productItemRepo.AddRange(validItems);

                gridAddForm.Visibility = Visibility.Collapsed;
                txtSerialList.Clear();
                LoadData();
                MessageBox.Show($"Thành công! Đã nhập kho {validItems.Count} thiết bị.", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống khi lưu: " + ex.Message);
            }
        }

        // ================= XỬ LÝ CHỈNH SỬA (EDIT) =================
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is ProductItem item)
            {
                if (item.Status != "InStock" && item.Status != "Defective")
                {
                    MessageBox.Show("Máy đã bán hoặc đang vận chuyển, không thể sửa!");
                    return;
                }
                _selectedItemForEdit = item;
                cbEditProduct.SelectedValue = item.ProductId;
                txtEditSerial.Text = item.SerialNumber;
                cbEditStatus.Text = item.Status;
                gridEditForm.Visibility = Visibility.Visible;
            }
        }

        private void btnConfirmUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cbEditProduct.SelectedValue == null) return;

            string newSerial = txtEditSerial.Text.Trim();
            int currentItemId = _selectedItemForEdit.ProductItemId;

            if (string.IsNullOrEmpty(newSerial))
            {
                MessageBox.Show("Số Serial không được để trống!", "Nhập liệu sai", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_productItemRepo.IsSerialDuplicate(newSerial, currentItemId))
            {
                MessageBox.Show($"Số Serial '{newSerial}' đã tồn tại trong hệ thống. Vui lòng nhập mã khác!",
                                "Trùng dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _selectedItemForEdit.ProductId = (int)cbEditProduct.SelectedValue;
                _selectedItemForEdit.SerialNumber = newSerial;
                _selectedItemForEdit.Status = (cbEditStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

                _productItemRepo.Update(_selectedItemForEdit);
                gridEditForm.Visibility = Visibility.Collapsed;
                LoadData();
                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra: " + ex.Message);
            }
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e) => gridEditForm.Visibility = Visibility.Collapsed;

        // ================= CÁC CHỨC NĂNG KHÁC =================
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is ProductItem item)
            {
                if (item.Status != "InStock" && item.Status != "Defective")
                {
                    MessageBox.Show($"Thiết bị đang ở trạng thái '{item.Status}', không được phép xóa!",
                                    "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Xác nhận xóa thiết bị này khỏi hệ thống?", "Xác nhận",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _productItemRepo.Delete(item.ProductItemId);
                        LoadData();
                        MessageBox.Show("Đã xóa thiết bị thành công.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void txtSearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            string k = txtSearchProduct.Text.ToLower();
            dgProductItems.ItemsSource = _fullList.Where(i => i.SerialNumber.ToLower().Contains(k) || i.Product.ProductName.ToLower().Contains(k)).ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
        private void btnGoToExport_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new OrderExportPage());
        private void btnViewReturned_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ReturnedOrderPage());

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}