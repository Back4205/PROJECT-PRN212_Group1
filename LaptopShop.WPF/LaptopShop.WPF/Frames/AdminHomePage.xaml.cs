using LaptopShop.Entities.Models;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class AdminHomePage : Page
    {
        private User? _currentUser;

        public AdminHomePage()
        {
            InitializeComponent();

            productTab.DataChanged += OnDataChanged;
            userTab.DataChanged += OnDataChanged;
            orderTab.DataChanged += OnDataChanged;
        }

        public AdminHomePage(User currentUser) : this()
        {
            _currentUser = currentUser;
        }

        private void OnDataChanged(object? sender, RoutedEventArgs e)
        {
            dashboardTab.RefreshData();
            orderTab.RefreshData();
        }
    }
}