using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LaptopShop.WPF.Frames
{
    public partial class OrderExportPage : Page
    {
        private readonly IOrderRepository _orderRepo;

        public OrderExportPage()
        {
            InitializeComponent();
            // Khởi tạo Repository (Đảm bảo bạn đã có class OrderRepository)
            _orderRepo = new OrderRepository();

            LoadData();
        }

        /// <summary>
        /// Lấy danh sách các đơn hàng Confirmed kèm theo OrderItems
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Gọi hàm từ Repository đã viết ở bước trước
                var confirmedOrders = _orderRepo.GetConfirmedOrdersWithItems();

                dgOrders.ItemsSource = confirmedOrders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đơn hàng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút Xác nhận xuất kho
        /// </summary>
        private void btnConfirmExport_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đối tượng Order từ DataContext của hàng được chọn
            var order = (sender as Button)?.DataContext as Order;

            if (order == null) return;

            // Kiểm tra xem tất cả OrderItems đã được gán ProductItemId chưa
            bool isAllAssigned = order.OrderItems.All(oi => oi.ProductItemId != null);

            if (!isAllAssigned)
            {
                MessageBox.Show("Không thể xuất kho! Vui lòng gán đầy đủ thiết bị (Serial) cho tất cả sản phẩm trong đơn hàng.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Xác nhận xuất kho cho đơn hàng #{order.OrderId}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Trong hàm btnConfirmExport_Click
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 1. Gọi hàm xử lý hoàn tất xuất kho từ Repository
                    _orderRepo.CompleteOrderExport(order.OrderId);

                    MessageBox.Show($"Đơn hàng #{order.OrderId} đã được xuất kho và chuyển trạng thái thiết bị thành 'sold'!",
                                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadData(); // Tải lại danh sách đơn hàng Confirmed
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất kho: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}