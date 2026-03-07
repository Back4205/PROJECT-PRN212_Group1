using LaptopShop.Services.Interfaces;
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
using System.Windows.Controls;
using LaptopShop.WPF.Frames;
namespace LaptopShop.WPF.Pages.Customer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly IProductService _productService;
        private User _user;
        public List<Product> FeaturedProducts { get; set; }
        public HomePage(User user)
        {
            InitializeComponent();
            _productService = new ProductService();
           
            _user = user;
            LoadData();
        }

        private void LoadData()
        {

            FeaturedProducts = _productService.GetAll().Where(p => p.IsActive == true).Take(3).ToList();
            this.DataContext = this;

        }

        private void BtnshopOutProducts_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new ProductPage(_user));
            }
        }

       
    }
    
}
