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

using MongoDB.Driver;
using MongoDB.Bson;

namespace Hotel_Management.Pages.QuanLyNhanSu
{
    /// <summary>
    /// Interaction logic for QuanLyNhanSu.xaml
    /// </summary>
    public partial class QuanLyNhanSu : Page
    {
        static MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
        static IMongoDatabase database = client.GetDatabase("HotelManagement");
        IMongoCollection<BsonDocument> collectionEmployee = database.GetCollection<BsonDocument>("User");
        List<NhanVien> employeeList = new List<NhanVien>();/* {
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


            };*/
        public QuanLyNhanSu()
        {
            InitializeComponent();
            LayNhanVien(collectionEmployee);
            textSoLuong.Text = "Số lượng: " + employeeList.Count.ToString();
            DGNhanVien.ItemsSource = employeeList;
            autoorder();
        }

        private void autoorder()
        {
            int t = 1;
            for (int i = 0; i < employeeList.Count; i++)
            {
                employeeList[i].stt = t;
                t++;
            }
        }

        public void LayNhanVien(IMongoCollection<BsonDocument> collectionEmployee)
        {
            string employeeName;
            string employeePhone;
            string employeeId;
            string employeeDoB;
            string employeeEmail;
            string employeeRole;
            List<BsonDocument> documentsEmployee = collectionEmployee.Find(new BsonDocument()).ToList();
            employeeList.Clear();
            foreach (BsonDocument employee in documentsEmployee)
            {
                employeeName = employee["userName"].AsString;
                employeeId = employee["idNumber"].AsString;
                employeePhone = employee["phoneNumber"].AsString;
                employeeDoB = employee["dateOfBirth"].ToLocalTime().ToShortDateString();
                employeeEmail = employee["email"].AsString;
                employeeRole = employee["userRole"].AsString;
                employeeList.Add(new NhanVien() { TenNV = employeeName, Sodienthoai = employeePhone, CCCD = employeeId, Ngaysinh = employeeDoB, Email = employeeEmail, Role = employeeRole });
            }

        }

        public class NhanVien
        {
            public int stt { get; set; }
            public string TenNV { get; set; }
            public string Sodienthoai { get; set; }
            public string CCCD { get; set; }
            public string Ngaysinh { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DGNhanVien.SelectedItems.Count != 0)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show("Xóa khách hàng đã chọn ?", "Cảnh báo", button, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    List<NhanVien> items = DGNhanVien.SelectedItems.Cast<NhanVien>().ToList();

                    //comment đến.....
                    foreach (NhanVien item in items)
                    {
                        employeeList.Remove(item);
                    }
                    DGNhanVien.ItemsSource = employeeList;
                    //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db


                    /* Xóa dữ liệu trong db
                    XoaNhanVien(collectionEmployee, items);
                    LayNhanVien(collectionEmployee);
                    */

                    textSoLuong.Text = "Số lượng: " + employeeList.Count.ToString();
                    DGNhanVien.ItemsSource = employeeList;
                    autoorder();
                    DGNhanVien.Items.Refresh();
                }
            }
        }

        public void XoaNhanVien(IMongoCollection<BsonDocument> collectionEmployee, List<NhanVien> items)
        {
            foreach (NhanVien item in items)
            {
                collectionEmployee.DeleteOne(x => x["idNumber"] == item.CCCD);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Xóa khách hàng?", "Cảnh báo", button, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NhanVien item = ((FrameworkElement)sender).DataContext as NhanVien;

                //comment đến.....
                employeeList.Remove(item);
                DGNhanVien.ItemsSource = employeeList;
                //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db

                /* Xóa dữ liệu trong db
                collectionEmployee.DeleteOne(x => x["idNumber"] == item.CCCD);
                LayNhanVien(collectionEmployee);
                */

                textSoLuong.Text = "Số lượng: " + employeeList.Count.ToString();
                DGNhanVien.ItemsSource = employeeList;
                autoorder();
                DGNhanVien.Items.Refresh();

            }
        }
    }
}
