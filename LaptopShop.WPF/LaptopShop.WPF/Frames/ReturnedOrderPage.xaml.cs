using LaptopShop.Entities.Models;
using LaptopShop.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LaptopShop.WPF.Frames
{
    public partial class ReturnedOrderPage : Page
    {
        // Sử dụng tên DbContext thật của bạn thay vì MyDbContext
        private readonly LaptopShopDbContext _context = new LaptopShopDbContext();

        public ReturnedOrderPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // 1. Xóa toàn bộ theo dõi cũ để đảm bảo lấy dữ liệu MỚI NHẤT từ SQL Server
                _context.ChangeTracker.Clear();

                var query = from s in _context.Shipments
                            join o in _context.Orders on s.OrderId equals o.OrderId
                            join c in _context.Customers on o.CustomerId equals c.CustomerId
                            join u in _context.Users on c.UserId equals u.UserId
                            join oi in _context.OrderItems on o.OrderId equals oi.OrderId
                            join pi in _context.ProductItems on oi.ProductItemId equals pi.ProductItemId
                            where s.Status == "Returned"
                                  && o.Status == "Cancelled"
                                  && pi.Status == "Sold" // Điều kiện này giúp dòng biến mất khi pi.Status đổi thành InStock
                            select new
                            {
                                s.ShipmentId,
                                o.OrderId,
                                CustomerName = u.FullName,
                                oi.SnapshotProductName,
                                pi.SerialNumber
                            };

                // 2. Gán dữ liệu và ép kiểu ToList để thực thi query ngay lập tức
                dgReturnedOrders.ItemsSource = query.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
        
    
        

        private void btnReStock_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            dynamic selectedRow = button?.DataContext;
            if (selectedRow == null) return;

            string serial = selectedRow.SerialNumber;

            var confirm = MessageBox.Show($"Nhập kho lại thiết bị: {serial}?", "Xác nhận", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    // Tìm sản phẩm dựa trên SerialNumber
                    var productItem = _context.ProductItems.FirstOrDefault(p => p.SerialNumber == serial);

                    // ... (đoạn code confirm của bạn)
                    if (productItem != null)
                    {
                        // Cập nhật
                        productItem.Status = "InStock";
                        _context.Entry(productItem).State = EntityState.Modified;
                        _context.SaveChanges();

                        MessageBox.Show("Đã nhập kho thành công!");

                        // ĐÂY LÀ BƯỚC QUAN TRỌNG: Load lại danh sách
                        LoadData();
                    }
                    // ...
                    else
                    {
                        MessageBox.Show("Không tìm thấy thiết bị này trong hệ thống.");
                    }
                }
                catch (Exception ex)
                {
                    // Hiển thị chi tiết lỗi InnerException nếu có (rất quan trọng để debug database)
                    var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show("Lỗi database: " + message);
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
    }
}