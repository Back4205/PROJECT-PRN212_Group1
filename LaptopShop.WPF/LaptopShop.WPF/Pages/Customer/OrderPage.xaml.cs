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
using LaptopShop.Services.Implementations;


namespace LaptopShop.WPF.Pages.Customer
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    
    public partial class OrderPage : Page
    {
        private User _user;
        private OrderService _orderService;
        private UserService _userService;
        public OrderPage( User user)
        {
            InitializeComponent();
            _user = user;
            _orderService = new OrderService();
            _userService = new UserService();
            LoadOrders();
        }
        private void LoadOrders()

        {
            var orders = _orderService.GetOrdersByCustomer(_user.UserId);

            icOrders.ItemsSource = orders;

        }
        private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Order order = btn.DataContext as Order;

            if (order.Status == "Pending")
            {
                _orderService.CancelOrder(order.OrderId);
                MessageBox.Show("Order cancelled!");

                LoadOrders();
            }
            else
            {
                MessageBox.Show("Cannot cancel this order.");
            }
        }
    }
}
