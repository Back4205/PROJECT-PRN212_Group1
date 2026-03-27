using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames.AdminTabs
{
    public partial class OrderManagementTab : UserControl
    {
        private readonly IOrderService _orderService;

        public event RoutedEventHandler? DataChanged;

        public OrderManagementTab()
        {
            InitializeComponent();
            _orderService = new OrderService();
            RefreshData();
        }

        public void RefreshData()
        {
            try
            {
                dgOrders.ItemsSource = null;
                dgOrders.ItemsSource = _orderService.GetAll();

                dgOrderItems.ItemsSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgOrders.SelectedItem is Order order)
                {
                    dgOrderItems.ItemsSource = null;
                    dgOrderItems.ItemsSource = _orderService.GetOrderItemsByOrderId(order.OrderId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}