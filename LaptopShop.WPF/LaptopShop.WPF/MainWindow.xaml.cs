using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using LaptopShop.WPF.Frames;
using LaptopShop.Entities.Models;

namespace LaptopShop.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private Role _currentRole;

        public MainWindow(User user, Role role)
        {
            InitializeComponent();

            _currentUser = user;
            _currentRole = role;

            LoadHomePageByRole();
        }

        private void LoadHomePageByRole()
        {
            switch (_currentRole.RoleName)
            {
                case "Admin":
                    MainFrame.Navigate(new AdminHomePage());
                    break;

                case "Staff":
                    MainFrame.Navigate(new StaffHomePage());
                    break;

                case "Customer":
                    MainFrame.Navigate(new CustomerHomePage());
                    break;
                case "Warehouse":
                    MainFrame.Navigate(new WarehouseHomePage());
                    break;

                default:
                    MessageBox.Show("Unknown role!");
                    break;
            }
        }
    }
}