using LaptopShop.Entities.Models;
using LaptopShop.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LaptopShop.WPF.Pages.Staff
{
    public partial class ShipmentList : Page
    {
        private readonly LaptopShopDbContext _context = new LaptopShopDbContext();
        private List<Shipment> _allShipments = new List<Shipment>();

        public ShipmentList()
        {
            InitializeComponent();
            this.Loaded += (s, e) => LoadShipments();
        }

        private void LoadShipments()
        {
            try
            {
                // Nạp đầy đủ dữ liệu liên quan để hiển thị tên khách và SĐT
                _allShipments = _context.Shipments
                    .Include(s => s.Order)
                        .ThenInclude(o => o.Customer)
                            .ThenInclude(c => c.User)
                    .OrderByDescending(s => s.ShipmentId)
                    .ToList();

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter()
        {
            if (!this.IsLoaded || dgShipments == null || txtSearch == null || cbStatusFilter == null) return;

            string query = txtSearch.Text;
            if (query == "Nhập tên hoặc số điện thoại...") query = "";
            query = query.ToLower().Trim();

            var selectedItem = cbStatusFilter.SelectedItem as ComboBoxItem;
            string status = selectedItem?.Content?.ToString() ?? "Tất cả";

            var filtered = _allShipments.Where(s =>
            {
                var user = s.Order?.Customer?.User;
                bool matchesSearch = string.IsNullOrEmpty(query) ||
                                    (user != null && (user.FullName.ToLower().Contains(query) || user.Phone.Contains(query)));

                bool matchesStatus = status == "Tất cả" || s.Status == status;
                return matchesSearch && matchesStatus;
            }).ToList();

            dgShipments.ItemsSource = filtered;
            dgShipments.Items.Refresh(); // Buộc giao diện vẽ lại
        }

        private void Filter_Changed(object sender, EventArgs e) => ApplyFilter();

        private void dgShipments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgShipments.SelectedItem is Shipment s)
            {
                // 🔥 Nạp kèm ProductItem để cột Số Serial có dữ liệu
                dgShipmentProducts.ItemsSource = _context.OrderItems
                    .Include(oi => oi.ProductItem)
                    .Where(oi => oi.OrderId == s.OrderId).ToList();
            }
            else dgShipmentProducts.ItemsSource = null;
        }

        private void btnStartShipping_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as Shipment;
            if (s != null && s.Status == "Preparing")
            {
                s.Status = "Shipping";
                s.ShipDate = DateTime.Now;
                _context.SaveChanges();
                LoadShipments();
            }
        }

        private void btnMarkDelivered_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as Shipment;
            if (s != null && s.Status == "Shipping")
            {
                if (MessageBox.Show($"Xác nhận giao thành công đơn #{s.OrderId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    s.Status = "Delivered";
                    var order = _context.Orders.FirstOrDefault(o => o.OrderId == s.OrderId);
                    if (order != null) order.Status = "Completed";

                    _context.SaveChanges();
                    LoadShipments();
                }
            }
        }

        // 🔥 FIX: LOGIC TRẢ HÀNG (Cho phép từ Shipping hoặc Delivered)
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as Button)?.DataContext as Shipment;
            if (s == null) return;

            if (s.Status == "Shipping" || s.Status == "Delivered")
            {
                if (MessageBox.Show("Xác nhận hoàn đơn hàng này? Đơn hàng sẽ chuyển sang trạng thái 'Chờ nhập kho'.",
                    "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var transaction = _context.Database.BeginTransaction();

                        // 1. Cập nhật Shipment sang Returned (để đánh dấu là hàng đang quay đầu)
                        s.Status = "Returned";

                        // 2. Cập nhật Order sang Cancelled
                        var order = _context.Orders.FirstOrDefault(o => o.OrderId == s.OrderId);
                        if (order != null) order.Status = "Cancelled";

                        _context.SaveChanges();
                        transaction.Commit();

                        MessageBox.Show("Đã xác nhận hoàn hàng thành công! Đơn hàng này sẽ được chuyển về danh sách chờ nhân viên kho xác nhận nhập kho lại.",
                                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadShipments(); // Refresh lại danh sách vận chuyển
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "Nhập tên hoặc số điện thoại...";
            txtSearch.Foreground = Brushes.Gray;
            cbStatusFilter.SelectedIndex = 0;
            LoadShipments();
            dgShipmentProducts.ItemsSource = null;
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Nhập tên hoặc số điện thoại...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Nhập tên hoặc số điện thoại...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }
    }
}