using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Frames
{
    public partial class AdminHomePage : Page
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        private List<Role> _allRoles = new List<Role>();

        public AdminHomePage()
        {
            InitializeComponent();

            _userService = new UserService();
            _productService = new ProductService();
            _orderService = new OrderService();

            LoadDashboard();
            LoadProducts();
            LoadUsers();
            LoadRoles();
        }

        private void LoadDashboard()
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

        private void LoadProducts()
        {
            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = _productService.GetAll();
        }

        private void LoadUsers()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = _userService.GetAllUsers();
        }

        private void LoadRoles()
        {
            _allRoles = _userService.GetAllRoles();
            lbRoles.ItemsSource = _allRoles;
        }

        private void btnLoadProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = _productService.Search(txtSearchProduct.Text);
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product product)
            {
                txtProductId.Text = product.ProductId.ToString();
                txtProductCode.Text = product.ProductCode;
                txtProductName.Text = product.ProductName;
                txtBrand.Text = product.Brand;
                txtBasePrice.Text = product.BasePrice.ToString();
                txtImgUrl.Text = product.ImgUrl;
                chkIsActive.IsChecked = product.IsActive;
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    ProductCode = txtProductCode.Text,
                    ProductName = txtProductName.Text,
                    Brand = txtBrand.Text,
                    BasePrice = decimal.Parse(txtBasePrice.Text),
                    ImgUrl = txtImgUrl.Text,
                    IsActive = chkIsActive.IsChecked ?? true
                };

                _productService.Add(product);
                MessageBox.Show("Thêm sản phẩm thành công.");

                LoadProducts();
                LoadDashboard();
                ClearProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    ProductId = int.Parse(txtProductId.Text),
                    ProductCode = txtProductCode.Text,
                    ProductName = txtProductName.Text,
                    Brand = txtBrand.Text,
                    BasePrice = decimal.Parse(txtBasePrice.Text),
                    ImgUrl = txtImgUrl.Text,
                    IsActive = chkIsActive.IsChecked ?? true
                };

                _productService.Update(product);
                MessageBox.Show("Cập nhật sản phẩm thành công.");

                LoadProducts();
                LoadDashboard();
                ClearProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductId.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm.");
                    return;
                }

                int id = int.Parse(txtProductId.Text);
                _productService.Delete(id);

                MessageBox.Show("Xử lý xóa sản phẩm thành công.");
                LoadProducts();
                LoadDashboard();
                ClearProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearProductForm_Click(object sender, RoutedEventArgs e)
        {
            ClearProductForm();
        }

        private void ClearProductForm()
        {
            txtProductId.Text = "";
            txtProductCode.Text = "";
            txtProductName.Text = "";
            txtBrand.Text = "";
            txtBasePrice.Text = "";
            txtImgUrl.Text = "";
            chkIsActive.IsChecked = true;
            dgProducts.SelectedItem = null;
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                txtUserId.Text = user.UserId.ToString();
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;

                lbRoles.UnselectAll();
                foreach (var role in user.Roles)
                {
                    var matchedRole = _allRoles.FirstOrDefault(r => r.RoleId == role.RoleId);
                    if (matchedRole != null)
                    {
                        lbRoles.SelectedItems.Add(matchedRole);
                    }
                }
            }
        }

        private void btnUpdateRoles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserId.Text))
                {
                    MessageBox.Show("Vui lòng chọn user.");
                    return;
                }

                int userId = int.Parse(txtUserId.Text);
                List<int> selectedRoleIds = lbRoles.SelectedItems
                    .Cast<Role>()
                    .Select(r => r.RoleId)
                    .ToList();

                _userService.UpdateUserRoles(userId, selectedRoleIds);

                MessageBox.Show("Cập nhật role thành công.");
                LoadUsers();
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLockUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserId.Text))
                {
                    MessageBox.Show("Vui lòng chọn user.");
                    return;
                }

                int userId = int.Parse(txtUserId.Text);
                _userService.SetUserActiveStatus(userId, false);

                MessageBox.Show("Khóa tài khoản thành công.");
                LoadUsers();
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUnlockUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserId.Text))
                {
                    MessageBox.Show("Vui lòng chọn user.");
                    return;
                }

                int userId = int.Parse(txtUserId.Text);
                _userService.SetUserActiveStatus(userId, true);

                MessageBox.Show("Mở khóa tài khoản thành công.");
                LoadUsers();
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}