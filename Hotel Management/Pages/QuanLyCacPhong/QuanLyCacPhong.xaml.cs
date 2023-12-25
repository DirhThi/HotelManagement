using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_Management.Pages.QuanLyCacPhong
{
    /// <summary>
    /// Interaction logic for QuanLyCacPhong.xaml
    /// </summary>
    public partial class QuanLyCacPhong : Page
    {
        List<Phong> phongList = new List<Phong> {
           new Phong() { maphong = "101", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "102", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "104", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "201", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "203", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "204", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "301", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "302", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "401", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "402", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "502", loaiphong = "Vip",trangthai = "Phòng trống"},
            new Phong() { maphong = "503", loaiphong = "Vip",trangthai = "Phòng trống"},
            new Phong() { maphong = "504", loaiphong = "Vip",trangthai = "Phòng trống"},
        };

        public QuanLyCacPhong()
        {
            InitializeComponent();
            phongIC.ItemsSource = phongList;

        }

       /* private void sortphong_click(object sender, RoutedEventArgs e)
        {
            if (sortphongtb.Text == "Mã phòng")
            {
                sortphongtb.Text = "Loại phòng";
                phongList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongIC.Items.Refresh();
            }
            else
            {
                sortphongtb.Text = "Mã phòng";
                phongList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongIC.Items.Refresh();
            }
        }*/

        private void Phong_click(object sender, RoutedEventArgs e)
        {
            Phong PhongDuocChon = (sender as Button).DataContext as Phong;
            // Lấy dữ liệu của phòng thôn qua item.maphong sau đó gán mã phòng,trạng thái nếu phòng đã đặt hoặc dã thuê phải có thông tin thuê, thiếu gì tự thêm vào
            string maphong = PhongDuocChon.maphong;
            string loaiphong = PhongDuocChon.loaiphong;
            string trangthai = "phongthue";
            string loaithue = "";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortCB.SelectedIndex==0)
            {
                phongList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongIC.Items.Refresh();
               
               
            }
            if(sortCB.SelectedIndex == 1)
            {
                phongList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongIC.Items.Refresh();
            }
            if (sortCB.SelectedIndex == 2)
            {
                phongList.Sort((left, right) => left.trangthai.CompareTo(right.trangthai));
                phongIC.Items.Refresh();
            }
        }
    }

    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string trangthai { get; set; }

    }
}
