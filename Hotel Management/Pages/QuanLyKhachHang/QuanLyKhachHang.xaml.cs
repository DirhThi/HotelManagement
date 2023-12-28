using Hotel_Management.MongoDatabase;
using Hotel_Management.StaticEvents;
using MongoDB.Bson;
using MongoDB.Driver;
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

namespace Hotel_Management.Pages.QuanLyKhachHang
{
    /// <summary>
    /// Interaction logic for QuanLyKhachHang.xaml
    /// </summary>
    public partial class QuanLyKhachHang : Page
    {
        MongoHandler handler = MongoHandler.GetInstance();
        List<Khachhang> users = new List<Khachhang> {
             new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
             new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
             new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
            new Khachhang() { TenKH="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com"},
        };
        public QuanLyKhachHang()
        {
            InitializeComponent();
            DGKhachhang.ItemsSource = users;
            SyncData();
            StaticEventHandler.OnCustomerUpdated += UpdateData;
            autoorder();
        }

        private void UpdateData()
        {
           SyncData();
        }

        public void SyncData()
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Customer");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument customer in documents)
                {
                    users.Add(new Khachhang()
                    {
                        TenKH = customer["customerName"].AsString,
                        Sodienthoai = customer["phoneNumber"].AsString,
                        CCCD = customer["idNumber"].AsString,
                        Ngaysinh = customer["dateOfBirth"].AsString,
                        Email = customer["email"].AsString,
                    });
                }
            }
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
            public string TenKH { get; set; }
            public string Sodienthoai { get; set; }
            public string CCCD { get; set; }
            public string Ngaysinh { get; set; }
            public string Email { get; set; }

        }
    }
}
