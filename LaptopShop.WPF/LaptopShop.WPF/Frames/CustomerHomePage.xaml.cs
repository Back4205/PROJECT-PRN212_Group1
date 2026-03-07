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
            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.HomePage(_user));
            _user = user;
            this.DataContext = _user;

            MainContentFrame.Navigate(new LaptopShop.WPF.Pages.Customer.HomePage(_user));

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
    }
}
