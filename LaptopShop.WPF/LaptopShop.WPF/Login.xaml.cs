using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Net.Sockets;
=======
>>>>>>> origin/Qui
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
<<<<<<< HEAD
using LaptopShop.Entities.Models;
using LaptopShop.Services;
using LaptopShop.Services.Implementations;
=======
>>>>>>> origin/Qui

namespace LaptopShop.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
<<<<<<< HEAD
        UserService _userService;
        public Login()
        {
            InitializeComponent();
            _userService = new UserService();
=======
        public Login()
        {
            InitializeComponent();
>>>>>>> origin/Qui
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            string userName = txtUserName.Text;
            string passWord = pwdPassword.Password;

            var user = _userService.Login(userName, passWord);

            if (user == null)
            {
                MessageBox.Show("Error email or password!!!",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            // 🔥 Kiểm tra role
            if (user.Roles == null || user.Roles.Count == 0)
            {
                MessageBox.Show("Your account has no role assigned!",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (user.Roles.Count == 1)
            {
                // 👉 Có 1 role → vào thẳng
                OpenMainWindow(user, user.Roles.First());
            }
            else
            {
                // 👉 Có nhiều role → mở màn hình chọn role
                SelectRole selectRole = new SelectRole(user);
                selectRole.Show();
                this.Close();
            }
=======

>>>>>>> origin/Qui
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.Hide();
            Register register = new Register();
            register.ShowDialog();

        }
        private void OpenMainWindow(User user, Role role)
        {
            this.Hide();

            MainWindow mainWindow = new MainWindow(user, role);
            mainWindow.Show();
        }
=======

        }
>>>>>>> origin/Qui
    }
}
