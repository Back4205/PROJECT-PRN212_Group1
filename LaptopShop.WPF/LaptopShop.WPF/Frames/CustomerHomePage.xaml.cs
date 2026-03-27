using LaptopShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LaptopShop.Entities.Models;
using System.Windows.Controls;
namespace LaptopShop.WPF.Frames
{
    /// <summary>
    /// Interaction logic for CustomerHomePage.xaml
    /// </summary>
    public partial class CustomerHomePage : Page
    {
        private User _user;
        public CustomerHomePage( User user)
        {
            InitializeComponent();
            _user = user;
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.HomePage(_user));
            
            this.DataContext = _user;

            //MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.HomePage(_user));

            // lấy ký tự đầu làm avata 
            if (_user != null)
            {
                if (!string.IsNullOrEmpty(_user.FullName))
                {
                    txtAvatar.Text = _user.FullName.Substring(0, 1).ToUpper();
                }
                else if (!string.IsNullOrEmpty(_user.Username))
                {
                    txtAvatar.Text = _user.Username.Substring(0, 1).ToUpper();
                }
            }
        }

        private void NavHome_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.HomePage(_user));
        }

        private void NavProduct_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.ProductPage(_user));
        }

        private void NavOrder_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.OrderPage(_user));
        }

        private void btnGoToCart_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.CartPage(_user));
        }

        private void LoadAvatar()
        {
            if (_user != null)
            {
                if (!string.IsNullOrEmpty(_user.FullName))
                    txtAvatar.Text = _user.FullName.Substring(0, 1).ToUpper();
                else if (!string.IsNullOrEmpty(_user.Username))
                    txtAvatar.Text = _user.Username.Substring(0, 1).ToUpper();
            }
        }

        // Sự kiện Click vào Avatar (Cần khớp với MouseLeftButtonUp trong XAML)
        private void Avatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Tìm ContextMenu đã định nghĩa trong XAML và hiển thị nó
                var contextMenu = border.ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = border;
                    contextMenu.IsOpen = true;
                }
            }
        }

        // Xử lý nút Update Information trong Menu
        private void MenuUpdate_Click(object sender, RoutedEventArgs e)
        {
            // 1. Khởi tạo màn hình UpdateInformation và truyền UserId vào
            UpdateInformation updateWin = new UpdateInformation(_user.UserId);

            // 2. Hiển thị màn hình UpdateInformation (dùng Show thay vì ShowDialog)
            updateWin.Show();

            // 3. Tìm Window đang chứa Page CustomerHomePage này
            Window parentWindow = Window.GetWindow(this);

            // 4. Đóng Window cha đó lại
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        // Xử lý nút Logout trong Menu
        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận",
                                                    MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 1. Khởi tạo màn hình Login (Tên class của bạn là Login)
                // Lưu ý: Kiểm tra namespace nếu cần (vd: LaptopShop.WPF.Login)
                Login loginWindow = new Login();

                // 2. Hiển thị màn hình Login lên
                loginWindow.Show();

                // 3. Tìm Window đang chứa Page hiện tại (CustomerHomePage)
                Window parentWindow = Window.GetWindow(this);

                // 4. Đóng cửa sổ trang chủ (MainWindow) lại
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
        }
    }
}
