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
    /// Interaction logic for SelectRole.xaml
    /// </summary>
    public partial class SelectRole : Window
    {
        private User _user;

        public SelectRole(User user)
        {
            InitializeComponent();
            _user = user;

            lstRoles.ItemsSource = _user.Roles;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = lstRoles.SelectedItem as Role;

            if (selectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!",
                     "Thông báo",
                     MessageBoxButton.OK,
                     MessageBoxImage.Warning);
                return;
            }

            MainWindow mainWindow = new MainWindow(_user, selectedRole);
            mainWindow.Show();

            this.Close();
        }
    }
}
