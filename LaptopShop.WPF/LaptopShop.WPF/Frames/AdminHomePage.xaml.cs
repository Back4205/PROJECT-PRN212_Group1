using LaptopShop.Entities.Models;
using System;
using System.Windows;
using System.Windows.Controls;
// Má nhớ thêm dòng này để gọi được màn hình Login
using LaptopShop.WPF;

namespace LaptopShop.WPF.Frames
{
    public partial class AdminHomePage : Page
    {
        private User? _currentUser;

        // Constructor mặc định
        public AdminHomePage()
        {
            InitializeComponent();

            // Đăng ký sự kiện để khi Product/Account thay đổi thì Dashboard tự Refresh
            productTab.DataChanged += OnDataChanged;
            userTab.DataChanged += OnDataChanged;
            orderTab.DataChanged += OnDataChanged;
        }

        // Constructor nhận User từ màn hình Login truyền sang
        public AdminHomePage(User currentUser) : this()
        {
            _currentUser = currentUser;

            // Hiển thị tên Admin lên Header (nhớ đặt x:Name="txtAdminName" trong XAML)
            if (_currentUser != null)
            {
                txtAdminName.Text = _currentUser.FullName;
            }
        }

        // Hàm xử lý khi có dữ liệu thay đổi ở các Tab con
        private void OnDataChanged(object? sender, RoutedEventArgs e)
        {
            dashboardTab.RefreshData();
            orderTab.RefreshData();
        }

        // --- CHỨC NĂNG LOGOUT ---
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // 1. Hiện thông báo xác nhận cho chuyên nghiệp
            MessageBoxResult result = MessageBox.Show(
                "Má có chắc muốn đăng xuất khỏi hệ thống không?",
                "Xác nhận đăng xuất",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 2. Khởi tạo lại màn hình Login
                    // Nếu class của má tên là LoginWindow thì đổi chữ Login thành LoginWindow nhé
                    Login loginWindow = new Login();
                    loginWindow.Show();

                    // 3. Đóng cửa sổ hiện tại (MainWindow) đang chứa Page này
                    Window parentWindow = Window.GetWindow(this);
                    if (parentWindow != null)
                    {
                        parentWindow.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đăng xuất: " + ex.Message);
                }
            }
        }
    }
}