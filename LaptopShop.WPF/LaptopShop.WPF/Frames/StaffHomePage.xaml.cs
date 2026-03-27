using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LaptopShop.Entities.Models;
using LaptopShop.WPF.Pages.Staff;

namespace LaptopShop.WPF.Frames
{
    public partial class StaffHomePage : Page
    {
        private User _user;

        public StaffHomePage(User user)
        {
            InitializeComponent();
            _user = user;
            this.DataContext = _user;

            LoadAvatar();

            // Mặc định hiển thị danh sách đơn hàng ngay khi vào
            try
            {
                MainContentFrame.Navigate(new OrderList());
            }
            catch (Exception)
            {
                // Phòng trường hợp trang OrderList đang bị lỗi XAML
            }
        }

        private void LoadAvatar()
        {
            if (_user != null && !string.IsNullOrEmpty(_user.FullName))
            {
                txtAvatar.Text = _user.FullName.Substring(0, 1).ToUpper();
            }
        }

        private void NavOrderList_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new OrderList());
        }

        private void NavShipment_Click(object sender, RoutedEventArgs e)
        {
             MainContentFrame.Navigate(new ShipmentList()); // Mở ra khi bạn đã tạo page này
        }

        private void Avatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.ContextMenu != null)
            {
                border.ContextMenu.PlacementTarget = border;
                border.ContextMenu.IsOpen = true;
            }
        }

        private void MenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateInformation updateWin = new UpdateInformation(_user.UserId);
            updateWin.Show();
            Window.GetWindow(this)?.Close();
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                new Login().Show();
                Window.GetWindow(this)?.Close();
            }
        }
    }
}