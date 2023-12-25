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
        List<Phong> phongtrongList = new List<Phong> {
           new Phong() { maphong = "101", loaiphong = "Standard"},
            new Phong() { maphong = "102", loaiphong = "Standard"},
            new Phong() { maphong = "104", loaiphong = "Deluxe"},
            new Phong() { maphong = "201", loaiphong = "Standard"},
            new Phong() { maphong = "203", loaiphong = "Standard"},
            new Phong() { maphong = "204", loaiphong = "Deluxe"},
            new Phong() { maphong = "301", loaiphong = "Deluxe"},
            new Phong() { maphong = "302", loaiphong = "Standard"},
            new Phong() { maphong = "401", loaiphong = "Deluxe"},
            new Phong() { maphong = "402", loaiphong = "Deluxe"},
            new Phong() { maphong = "502", loaiphong = "Vip"},
            new Phong() { maphong = "503", loaiphong = "Vip"},
            new Phong() { maphong = "504", loaiphong = "Vip"},
        };

        List<Phong> phongthueList = new List<Phong> {
              new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
        };

        List<Phong> phongdatList = new List<Phong> {
             new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909"},
        };

        List<Phong> phongbaotriList = new List<Phong> {
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Đang bảo trì"},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Đang bảo trì"},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Đang bảo trì"},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Đang bảo trì"},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Đang bảo trì"},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Đang dọn dẹp"},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Đang bảo trì"},
        };
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

    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string tenkhachhang { get; set; }
        public string sodienthoai { get; set; }


    }


}
