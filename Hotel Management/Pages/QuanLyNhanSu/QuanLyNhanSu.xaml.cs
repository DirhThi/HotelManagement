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

namespace Hotel_Management.Pages.QuanLyNhanSu
{
    /// <summary>
    /// Interaction logic for QuanLyNhanSu.xaml
    /// </summary>
    public partial class QuanLyNhanSu : Page
    {
        List<Khachhang> users = new List<Khachhang> {
             new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
             new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
             new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},


            };
        public QuanLyNhanSu()
        {
            InitializeComponent();
            DGNhanVien.ItemsSource = users;
            autoorder();
        }

        private void autoorder()
        {
            int t = 1;
            for (int i = 0; i < users.Count; i++)
            {
                users[i].stt = t;
                t++;
            }
        }

        public class Khachhang
        {
            public int stt { get; set; }
            public string TenNV { get; set; }
            public string Sodienthoai { get; set; }
            public string CCCD { get; set; }
            public string Ngaysinh { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }


        }
    }
}
