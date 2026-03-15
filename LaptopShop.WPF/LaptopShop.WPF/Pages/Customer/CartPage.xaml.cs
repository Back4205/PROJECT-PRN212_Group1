using LaptopShop.Entities.Models;
using LaptopShop.Repositories.Implementations;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Pages.Customer
{
    public partial class CartPage : Page
    {
        private readonly User _user;
        private readonly CartService _cartService;
        private readonly OrderService _orderService; // Thêm OrderService
        private Cart _currentCart;

        public CartPage(User user)
        {
            InitializeComponent();
            _user = user;

            // Khởi tạo theo đúng cấu trúc: Repo -> Service
            var cartRepo = new CartRepository();
            var userRepo = new UserRepository();
            var orderRepo = new OrderRepository(); // Khởi tạo thêm OrderRepo

            _cartService = new CartService(cartRepo, userRepo);

            // Khởi tạo OrderService với các dependencies cần thiết
            _orderService = new OrderService();

            LoadCartData();
        }

        // 1. Tải dữ liệu giỏ hàng lên DataGrid
        private void LoadCartData()
        {
            try
            {
                _currentCart = _cartService.GetCartByUserId(_user.UserId);

                if (_currentCart != null)
                {
                    // Gán nguồn dữ liệu cho DataGrid dgCart
                    // Vì không có Quantity, mỗi item trong giỏ là một máy riêng biệt
                    dgCart.ItemsSource = _currentCart.CartItems.ToList();
                    UpdateTotalAmount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải giỏ hàng: " + ex.Message);
            }
        }

        // 2. Tính toán tổng tiền (Cập nhật: Bỏ Quantity, tính trực tiếp BasePrice)
        private void UpdateTotalAmount()
        {
            if (_currentCart?.CartItems != null && _currentCart.CartItems.Any())
            {
                // Mỗi dòng là 1 máy, nên tổng tiền = tổng BasePrice của các item
                decimal total = _currentCart.CartItems.Sum(item => item.Product.BasePrice * item.Quantity);
                txtTotalAmount.Text = string.Format("{0:N0} VND", total);
            }
            else
            {
                txtTotalAmount.Text = "0 VND";
            }
        }

        // 3. Xử lý xóa sản phẩm (Delete)
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int cartItemId)
            {
                var result = MessageBox.Show("Bạn có muốn xóa máy này khỏi giỏ hàng?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _cartService.RemoveFromCart(cartItemId);
                    LoadCartData(); // Tải lại dữ liệu
                }
            }
        }

        // 4. Xử lý Đặt hàng (ORDER NOW)
        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCart == null || !_currentCart.CartItems.Any())
            {
                MessageBox.Show("Giỏ hàng của bạn đang trống!");
                return;
            }

            var confirm = MessageBox.Show("Xác nhận đặt hàng với phương thức Thanh toán khi nhận hàng (COD)?",
                                        "Xác nhận đơn hàng", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    // Gọi hàm PlaceOrder đã viết ở tầng Service
                    // Hàm này sẽ tự động: Snapshot tên/giá, tạo Order, xóa Cart
                    _orderService.PlaceOrder(_user.UserId);

                    MessageBox.Show("Đặt hàng thành công! Đơn hàng đang chờ nhân viên kho gán số Serial.", "Thông báo");

                    // Quay về trang Lịch sử đơn hàng (OrderPage)
                    NavigationService.Navigate(new OrderPage(_user));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi đặt hàng: " + ex.Message);
                }
            }
        }
        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            CartItem item = btn.DataContext as CartItem;

            item.Quantity++;

            CartRepository repo = new CartRepository();
            repo.UpdateQuantity(item.CartItemId, item.Quantity);

            dgCart.Items.Refresh();
            UpdateTotalAmount();
        }
        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            CartItem item = btn.DataContext as CartItem;

            if (item.Quantity > 1)
            {
                item.Quantity--;

                CartRepository repo = new CartRepository();
                repo.UpdateQuantity(item.CartItemId, item.Quantity);

                dgCart.Items.Refresh();
                UpdateTotalAmount();
            }
            else
            {
                
                MessageBox.Show("Quantity cannot be less than 1!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductPage(_user));
        }
    }
}