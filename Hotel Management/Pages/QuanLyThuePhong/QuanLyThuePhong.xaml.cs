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
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" } },
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},

        };

        List<Phong> phongthueList = new List<Phong> {
             new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" } },
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
        };

        List<Phong> phongdatList = new List<Phong> {
             new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" } },
            new Phong() { maphong = "102", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "104", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh","Máy lạnh","Wifi" }},
            new Phong() { maphong = "201", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "203", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "204", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "301", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "302", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "401", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "402", loaiphong = "Deluxe",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "502", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "503", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "504", loaiphong = "Vip",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
        };

        List<Phong> phongbaotriList = new List<Phong> {
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Đang dọn dẹp",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" }},

        };
        public QuanLyThuePhong()
        {
            InitializeComponent();
            //lấy dữ liệu các phòng thêm vào list tương ứng đã tạo bên trên .
            phongtrongIC.ItemsSource = phongtrongList;
            phongthueIC.ItemsSource = phongthueList;
            phongdatIC.ItemsSource = phongdatList;
            phongbaotriIC.ItemsSource = phongbaotriList;
        }

        private void Phong_click(object sender, RoutedEventArgs e)
        {
            Phong PhongDuocChon = (sender as Button).DataContext as Phong;
            // Lấy dữ liệu của phòng thôn qua item.maphong sau đó gán mã phòng,trạng thái nếu phòng đã đặt hoặc dã thuê phải có thông tin thuê, thiếu gì tự thêm vào
            string maphong = PhongDuocChon.maphong;
            string loaiphong = PhongDuocChon.loaiphong;
            string trangthai = "phongthue";
            string loaithue = "";

            quanlythuephong.NavigationService.Navigate(new ChiTietPhong(maphong,loaiphong,trangthai, loaithue));
        }

        private void Phongbaotri_click(object sender, RoutedEventArgs e)
        {

            Dialog.IsOpen = true;
        }

        private void sortphongtrong_click(object sender, RoutedEventArgs e)
        {
            if(sortphongtrongtb.Text=="Mã phòng")
            {
                sortphongtrongtb.Text = "Loại phòng";
                phongtrongList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongtrongIC.Items.Refresh();
            }
            else
            {
                sortphongtrongtb.Text = "Mã phòng";
                phongtrongList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongtrongIC.Items.Refresh();
            }
        }

        private void sortphongthue_click(object sender, RoutedEventArgs e)
        {
            if (sortphongthuetb.Text == "Mã phòng")
            {
                sortphongthuetb.Text = "Loại phòng";
                phongthueList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongthueIC.Items.Refresh();
            }
            else
            {
                sortphongthuetb.Text = "Mã phòng";
                phongthueList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongthueIC.Items.Refresh();
            }
        }

        private void sortphongdat_click(object sender, RoutedEventArgs e)
        {
            if (sortphongdattb.Text == "Mã phòng")
            {
                sortphongdattb.Text = "Loại phòng";
                phongdatList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongdatIC.Items.Refresh();
            }
            else
            {
                sortphongdattb.Text = "Mã phòng";
                phongdatList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongdatIC.Items.Refresh();
            }
        }

        private void sortphongbaotri_click(object sender, RoutedEventArgs e)
        {
            if (sortphongbaotritb.Text == "Mã phòng")
            {
                sortphongbaotritb.Text = "Loại phòng";
                phongbaotriList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongbaotriIC.Items.Refresh();
            }
            else
            {
                sortphongbaotritb.Text = "Mã phòng";
                phongbaotriList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongbaotriIC.Items.Refresh();
            }
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;
        }
    }

    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string tenkhachhang { get; set; }
        public string sodienthoai { get; set; }
        public List<string> ListCsvc { get; set; }

    }


}
