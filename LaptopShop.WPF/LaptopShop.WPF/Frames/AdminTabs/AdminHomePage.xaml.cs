using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class AdminHomePage : Page
    {
        public AdminHomePage()
        {
            InitializeComponent();

            productTab.DataChanged += OnDataChanged;
            userTab.DataChanged += OnDataChanged;
            orderTab.DataChanged += OnDataChanged;
        }

        private void OnDataChanged(object? sender, RoutedEventArgs e)
        {
            dashboardTab.RefreshData();
            orderTab.RefreshData();
        }
    }
}