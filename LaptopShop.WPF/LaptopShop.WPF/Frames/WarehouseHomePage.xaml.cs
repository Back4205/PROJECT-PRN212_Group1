using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class WarehouseHomePage : Page
    {
        private User _user;
        private readonly WarehouseRepository _warehouseRepository;

        public WarehouseHomePage(User user)
        {
            InitializeComponent();
            _user = user;
            _warehouseRepository = new WarehouseRepository();
            LoadWarehouseData();
        }

        private void LoadWarehouseData()
        {
            // Lấy danh sách kho mà User này làm Manager
            var myWarehouses = _warehouseRepository.GetAll()
                                .Where(w => w.ManagerUserId == _user.UserId)
                                .ToList();

            if (myWarehouses != null && myWarehouses.Count > 0)
            {
                cbWarehouses.ItemsSource = myWarehouses;
                cbWarehouses.SelectedIndex = 0;
            }
            else
            {
                txtStatus.Text = "Tài khoản này chưa được gán quản lý kho nào.";
                btnSelectWarehouse.IsEnabled = false;
            }
        }

        private void btnSelectWarehouse_Click(object sender, RoutedEventArgs e)
        {
            // Thay đổi quan trọng ở đây:
            if (cbWarehouses.SelectedItem is Warehouse selectedWarehouse)
            {
                // 1. Lấy WarehouseId từ kho được chọn
                int selectedId = selectedWarehouse.WarehouseId;

                // 2. Thực hiện điều hướng sang trang ProductItemPage 
                // và truyền WarehouseId vào Constructor của trang đó
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new ProductItemPage(selectedId));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một kho hàng!");
            }
        }
    }
}