using LaptopShop.WPF.Frames;
using System.Windows;

namespace LaptopShop.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AdminHomePage());
        }
    }
}