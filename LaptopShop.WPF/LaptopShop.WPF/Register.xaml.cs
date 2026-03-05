using System;
using System.Windows;
using System.Windows.Input;
using LaptopShop.Services.Implementations;
using LaptopShop.Entities.Models;

namespace LaptopShop.WPF
{
    public partial class Register : Window
    {
        private readonly UserService _userService;

        public Register()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1️⃣ Validate cơ bản ở UI
                if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                    string.IsNullOrWhiteSpace(pwdPassword.Password) ||
                    string.IsNullOrWhiteSpace(pwdConfirmPassword.Password) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                if (pwdPassword.Password != pwdConfirmPassword.Password)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp.");
                    return;
                }

                // 2️⃣ Tạo User object
                var newUser = new User
                {
                    Username = txtUserName.Text.Trim(),
                    PasswordHash = pwdPassword.Password, // plain password (Service sẽ hash)
                    FullName = txtFullname.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhoneNumber.Text.Trim(),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                // 3️⃣ Gọi Service xử lý
                _userService.Register(newUser);

                MessageBox.Show("Đăng ký tài khoản thành công!");

                // 4️⃣ Quay lại Login
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                // Chỉ hiển thị message
                MessageBox.Show(ex.Message);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}