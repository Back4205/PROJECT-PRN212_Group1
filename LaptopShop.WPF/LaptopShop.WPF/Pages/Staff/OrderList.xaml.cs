using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LaptopShop.WPF.Pages.Staff
{
    public partial class OrderList : Page
    {
        private readonly IOrderService _orderService;
        private List<Order> _allOrders = new List<Order>();

        public OrderList()
        {
            InitializeComponent();
            _orderService = new OrderService();

            this.Loaded += OrderList_Loaded;
        }

        private void OrderList_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                _allOrders = _orderService.GetAll() ?? new List<Order>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đơn hàng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter()
        {

            if (_allOrders == null || txtSearchName == null || cbStatusFilter == null)
                return;


            string searchText = txtSearchName.Text;
            if (searchText == "Nhập tên khách hàng...") searchText = "";
            searchText = searchText.ToLower().Trim();

            // 2. Lấy giá trị Status từ ComboBox an toàn
            var selectedItem = cbStatusFilter.SelectedItem as ComboBoxItem;
            string selectedStatus = selectedItem?.Content?.ToString() ?? "Tất cả";

            // 3. Thực hiện lọc
            var filteredData = _allOrders.Where(o =>
            {
                bool matchesName = string.IsNullOrEmpty(searchText) ||
                                  (o.Customer?.User?.FullName != null &&
                                   o.Customer.User.FullName.ToLower().Contains(searchText));

                bool matchesStatus = selectedStatus == "Tất cả" || o.Status == selectedStatus;

                return matchesName && matchesStatus;
            }).ToList();

            dgOrders.ItemsSource = filteredData;
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            // Chỉ chạy lọc khi trang đã load xong hoàn toàn
            if (this.IsLoaded)
            {
                ApplyFilter();
            }
        }

        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is Order selectedOrder)
            {
                dgOrderItems.ItemsSource = selectedOrder.OrderItems?.ToList();
            }
            else
            {
                dgOrderItems.ItemsSource = null;
            }
        }

        private void txtSearchName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchName.Text == "Nhập tên khách hàng...")
            {
                txtSearchName.Text = "";
                txtSearchName.Foreground = Brushes.Black;
            }
        }

        private void txtSearchName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                txtSearchName.Text = "Nhập tên khách hàng...";
                txtSearchName.Foreground = Brushes.Gray;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.DataContext as Order;

            if (order != null)
            {
                if (order.Status != "Pending")
                {
                    MessageBox.Show("Chỉ có thể xác nhận đơn hàng đang ở trạng thái Pending!", "Thông báo");
                    return;
                }

                if (MessageBox.Show($"Xác nhận đơn hàng #{order.OrderId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    order.Status = "Confirmed";
                    _orderService.Update(order);
                    MessageBox.Show("Xác nhận đơn hàng thành công!");
                    LoadOrders();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.DataContext as Order;

            if (order != null)
            {
                if (order.Status != "Pending")
                {
                    MessageBox.Show("Chỉ có thể hủy đơn hàng đang ở trạng thái Pending!", "Thông báo");
                    return;
                }

                if (MessageBox.Show($"Bạn có chắc muốn HỦY đơn hàng #{order.OrderId}? Sản phẩm sẽ được trả lại kho.",
                                    "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //bool success = _orderService.CancelOrder(order.OrderId);
                    //if (success)
                    //{
                    //    MessageBox.Show("Đã hủy đơn hàng và hoàn trả kho thành công!");
                    //    LoadOrders();
                    //}
                    //else MessageBox.Show("Hủy đơn hàng thất bại!");
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearchName != null)
            {
                txtSearchName.Text = "Nhập tên khách hàng...";
                txtSearchName.Foreground = Brushes.Gray;
            }
            if (cbStatusFilter != null)
            {
                cbStatusFilter.SelectedIndex = 0;
            }
            LoadOrders();
            dgOrderItems.ItemsSource = null;
        }
    }
}

