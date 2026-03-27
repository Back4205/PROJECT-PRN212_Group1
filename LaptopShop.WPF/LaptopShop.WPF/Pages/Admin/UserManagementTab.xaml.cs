using LaptopShop.Entities.Models;
using LaptopShop.Services.Implementations;
using LaptopShop.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace LaptopShop.WPF.Pages.Admin
{
    public partial class UserManagementTab : UserControl
    {
        private readonly IUserService _userService;
        private List<Role> _allRoles = new List<Role>();

        public event RoutedEventHandler? DataChanged;

        public UserManagementTab()
        {
            InitializeComponent();
            _userService = new UserService();

            LoadUsers();
            LoadRoles();
        }

        private void LoadUsers()
        {
            try
            {
                dgUsers.ItemsSource = null;
                dgUsers.ItemsSource = _userService.GetAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadRoles()
        {
            try
            {
                _allRoles = _userService.GetAllRoles();
                lbRoles.ItemsSource = _allRoles;
                lbNewUserRoles.ItemsSource = _allRoles;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                lbRoles.SelectedItems.Clear();

                foreach (var role in _allRoles)
                {
                    if (user.Roles.Any(r => r.RoleId == role.RoleId))
                    {
                        lbRoles.SelectedItems.Add(role);
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

                var roleIds = lbRoles.SelectedItems
                    .Cast<Role>()
                    .Select(r => r.RoleId)
                    .ToList();

                _userService.UpdateUserRoles(userId, roleIds);

                MessageBox.Show("Cập nhật role thành công.");
                LoadUsers();
                DataChanged?.Invoke(this, new RoutedEventArgs());
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

                int id = int.Parse(txtUserId.Text);
                _userService.SetUserActiveStatus(id, false);

                MessageBox.Show("Khóa tài khoản thành công.");
                LoadUsers();
                DataChanged?.Invoke(this, new RoutedEventArgs());
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

                int id = int.Parse(txtUserId.Text);
                _userService.SetUserActiveStatus(id, true);

                MessageBox.Show("Mở khóa tài khoản thành công.");
                LoadUsers();
                DataChanged?.Invoke(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var roleIds = lbNewUserRoles.SelectedItems
                    .Cast<Role>()
                    .Select(r => r.RoleId)
                    .ToList();

                User user = new User
                {
                    Username = txtNewUsername.Text,
                    PasswordHash = txtNewPassword.Password,
                    FullName = txtNewFullName.Text,
                    Email = txtNewEmail.Text,
                    Phone = txtNewPhone.Text
                };

                _userService.AddUserByAdmin(user, roleIds);

                MessageBox.Show("Thêm user thành công.");
                LoadUsers();
                ClearAddUserForm();
                DataChanged?.Invoke(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearAddUserForm()
        {
            txtNewUsername.Text = "";
            txtNewPassword.Password = "";
            txtNewFullName.Text = "";
            txtNewEmail.Text = "";
            txtNewPhone.Text = "";
            lbNewUserRoles.UnselectAll();
        }
    }
}