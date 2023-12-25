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


namespace Hotel_Management.Pages.QuanLyThuePhong
{
    /// <summary>
    /// Interaction logic for QuanLyThuePhong.xaml
    /// </summary>
    public partial class QuanLyThuePhong : Page
    {
        List<int> phongItems = new List<int>();
        List<int> numberList = new List<int>() { 101, 102, 103, 201, 203, 301, 304, 401, 402, 403 };
        public QuanLyThuePhong()
        {
            InitializeComponent();
            phongtrongtang1IC.ItemsSource = numberList;
        }

        private void Phong_click(object sender, RoutedEventArgs e)
        {
            var item = (Button)sender;
            string maphong = "105";
            quanlythuephong.NavigationService.Navigate(new ChiTietPhong(maphong));
        }
    }
}
