using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames.AdminTabs
{
    public partial class DashboardTab : UserControl
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public DashboardTab()
        {
            InitializeComponent();

            _userService = new UserService();
            _productService = new ProductService();
            _orderService = new OrderService();

            RefreshData();
        }

        public void RefreshData()
        {
            try
            {
                var users = _userService.GetAllUsers();
                var products = _productService.GetAll();

                txtTotalUsers.Text = users.Count.ToString();
                txtActiveUsers.Text = users.Count(u => u.IsActive).ToString();
                txtTotalProducts.Text = products.Count.ToString();
                txtActiveProducts.Text = products.Count(p => p.IsActive).ToString();
                txtTotalOrders.Text = _orderService.GetTotalOrders().ToString();
                txtRevenue.Text = _orderService.GetTotalRevenue().ToString("N0");

                txtPendingOrders.Text = _orderService.GetOrderCountByStatus("Pending").ToString();
                txtConfirmedOrders.Text = _orderService.GetOrderCountByStatus("Confirmed").ToString();
                txtCancelledOrders.Text = _orderService.GetOrderCountByStatus("Cancelled").ToString();
                txtCompletedOrders.Text = _orderService.GetOrderCountByStatus("Completed").ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard lỗi: " + ex.Message);
            }
        }
    }
}