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
        static MongoHandler handler = MongoHandler.GetInstance();
        IMongoCollection<BsonDocument> collectionCustomer = handler.GetCollection("Customer");
        List<Khachhang> customerList = new List<Khachhang>(); /*{
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
        };*/
        public QuanLyKhachHang()
        {
            InitializeComponent();
            
            LayKhachHang(collectionCustomer);
            DGKhachhang.ItemsSource = customerList;
            textSoLuong.Text = "Số lượng: " + customerList.Count.ToString();
            //SyncData();
            //StaticEventHandler.OnCustomerUpdated += UpdateData;
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
                    customerList.Add(new Khachhang()
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
            for (int i = 0; i < customerList.Count; i++)
            {
                customerList[i].stt = t;
                t++;
            }
        }

        public void LayKhachHang(IMongoCollection<BsonDocument> collectionCustomer)
        {
            string customerName;
            string customerPhone;
            string customerId;
            string customerDoB;
            string customerEmail;
            List<BsonDocument> documentsCustomer = collectionCustomer.Find(new BsonDocument()).ToList();
            customerList.Clear();
            foreach(BsonDocument customer in documentsCustomer)
            {
                customerName = customer["customerName"].AsString;
                customerPhone = customer["phoneNumber"].AsString;
                customerId = customer["idNumber"].AsString;
                customerDoB = customer["dateOfBirth"].ToLocalTime().ToShortDateString();
                customerEmail = customer["email"].AsString;

                customerList.Add(new Khachhang() { TenKH = customerName, Sodienthoai = customerPhone, CCCD = customerId, Ngaysinh = customerDoB, Email = customerEmail });
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DGKhachhang.SelectedItems.Count != 0)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show("Xóa khách hàng đã chọn ?", "Cảnh báo", button, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    List<Khachhang> items = DGKhachhang.SelectedItems.Cast<Khachhang>().ToList();
                    
                    //comment đến.....
                    foreach (Khachhang item in items)
                    {
                        customerList.Remove(item);
                    }
                    DGKhachhang.ItemsSource = customerList;
                    //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db
                    

                    /* Xóa dữ liệu trong db
                    XoaKhachHang(collectionCustomer, items);
                    LayKhachHang(collectionCustomer);
                    */

                    textSoLuong.Text = "Số lượng: " + customerList.Count.ToString();
                    DGKhachhang.ItemsSource = customerList;
                    autoorder();
                    DGKhachhang.Items.Refresh();

                }
            }
        }

        public void XoaKhachHang(IMongoCollection<BsonDocument> collectionCustomer, List<Khachhang> items)
        {
            foreach (Khachhang item in items)
            {
                collectionCustomer.DeleteOne(x => x["idNumber"] == item.CCCD);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Xóa khách hàng?", "Cảnh báo", button, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Yes)
            {
                Khachhang item = ((FrameworkElement)sender).DataContext as Khachhang;

                //comment đến.....
                customerList.Remove(item);
                DGKhachhang.ItemsSource = customerList;
                //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db

                /* Xóa dữ liệu trong db
                collectionCustomer.DeleteOne(x => x["idNumber"] == item.CCCD);
                LayKhachHang(collectionCustomer);
                */

                textSoLuong.Text = "Số lượng: " + customerList.Count.ToString();
                DGKhachhang.ItemsSource = customerList;
                autoorder();
                DGKhachhang.Items.Refresh();

            }
        }
    }
}
