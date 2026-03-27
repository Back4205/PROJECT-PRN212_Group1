using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
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
using System.Windows.Shapes;
using LaptopShop.Entities.Models;
namespace LaptopShop.WPF
{
    /// <summary>
    /// Interaction logic for UpdateInformation.xaml
    /// </summary>
    public partial class UpdateInformation : Window
    {
        private readonly IUserService _userService;
        private User _currentUser;

        // Giả sử bạn truyền UserId vào khi mở Window này
        public UpdateInformation(int userId)
        {
            InitializeComponent();
            _userService = new UserService();
            LoadData(userId);
        }

        private void LoadData(int userId)
        {
            _currentUser = _userService.GetUserById(userId);
            if (_currentUser == null) return;

            // Đổ dữ liệu vào TextBox
            txtFullName.Text = _currentUser.FullName;
            txtEmail.Text = _currentUser.Email;
            txtPhone.Text = _currentUser.Phone;

            // Kiểm tra Role để hiện địa chỉ
            var roles = _userService.GetRolesByUserId(userId);
            if (roles.Any(r => r.RoleName == "Customer"))
            {
                spCustomerAddress.Visibility = Visibility.Visible;
                var customer = _userService.GetCustomerByUserId(userId);
                txtAddress.Text = customer?.Address;
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validate sơ bộ
                if (string.IsNullOrWhiteSpace(txtFullName.Text)) throw new Exception("Họ tên không được để trống.");

                // 2. Kiểm tra mật khẩu khớp
                string newPass = txtNewPassword.Password;
                if (!string.IsNullOrEmpty(newPass) && newPass != txtConfirmPassword.Password)
                {
                    MessageBox.Show("Xác nhận mật khẩu không khớp!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 3. Gán giá trị mới cho đối tượng User
                _currentUser.FullName = txtFullName.Text;
                _currentUser.Email = txtEmail.Text;
                _currentUser.Phone = txtPhone.Text;

                // 4. Gọi Service cập nhật
                string address = spCustomerAddress.Visibility == Visibility.Visible ? txtAddress.Text : null;

                _userService.UpdateUserProfile(_currentUser, newPass, address);

                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // 🔥 FIX: Sau khi hiện thông báo thành công, phải gọi hàm quay về Home
                BackToHome();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Nhấn Hủy cũng quay về Home
            BackToHome();
        }

        // Hàm này tự động điều hướng dựa trên Role của User
        private void BackToHome()
        {
            try
            {
                // 1. Lấy lại thông tin mới nhất từ DB
                var updatedUser = _userService.GetUserById(_currentUser.UserId);
                var roles = _userService.GetRolesByUserId(_currentUser.UserId);

                if (roles != null && roles.Count > 0)
                {
                    // 2. Tối ưu: Ưu tiên chọn Role "Customer" nếu có trong danh sách
                    // Nếu không có Customer, mới lấy cái đầu tiên
                    var currentRole = roles.FirstOrDefault(r => r.RoleName == "Customer") ?? roles.First();

                    // 3. Khởi tạo lại MainWindow với dữ liệu đã cập nhật
                    MainWindow mainWindow = new MainWindow(updatedUser, currentRole);

                    // 4. Hiển thị lại giao diện chính
                    mainWindow.Show();

                    // 5. Đóng cửa sổ UpdateInformation hiện tại
                    this.Close();
                }
                else
                {
                    // Nếu không có role nào, bắt buộc quay về Login
                    Login login = new Login();
                    login.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi quay lại giao diện chính: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                // Safety fallback: Quay về Login nếu lỗi nặng
                new Login().Show();
                this.Close();
            }
        }
    }
}
