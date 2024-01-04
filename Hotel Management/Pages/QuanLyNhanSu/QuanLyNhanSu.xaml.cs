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
            };*/
        List<NhanVien> employeeListDisplay = new List<NhanVien>();
        public QuanLyNhanSu()
        {
            InitializeComponent();
            LayNhanVien(collectionEmployee);
            textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
            DGNhanVien.ItemsSource = employeeListDisplay;
            autoorder();
        }

        private void autoorder()
        {
            int t = 1;
            for (int i = 0; i < employeeListDisplay.Count; i++)
            {
                employeeListDisplay[i].stt = t;
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
            employeeListDisplay.Clear();

            foreach (BsonDocument employee in documentsEmployee)
            {
                employeeName = employee["userName"].AsString;
                employeeId = employee["idNumber"].AsString;
                employeePhone = employee["phoneNumber"].AsString;
                employeeDoB = employee["dateOfBirth"].ToLocalTime().ToShortDateString();
                employeeEmail = employee["email"].AsString;
                employeeRole = employee["userRole"].AsString;
                employeeList.Add(new NhanVien() { TenNV = employeeName, Sodienthoai = employeePhone, CCCD = employeeId, Ngaysinh = employeeDoB, Email = employeeEmail, Role = employeeRole });
                employeeListDisplay.Add(new NhanVien() { TenNV = employeeName, Sodienthoai = employeePhone, CCCD = employeeId, Ngaysinh = employeeDoB, Email = employeeEmail, Role = employeeRole });

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
                MessageBoxResult result = MessageBox.Show("Xóa nhân viên đã chọn ?", "Cảnh báo", button, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    List<NhanVien> items = DGNhanVien.SelectedItems.Cast<NhanVien>().ToList();

                    //comment đến.....
                    foreach (NhanVien item in items)
                    {
                        employeeList.Remove(item);
                        employeeListDisplay.Remove(item);

                    }
                    DGNhanVien.ItemsSource = employeeListDisplay;
                    //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db


                    /* Xóa dữ liệu trong db
                    XoaNhanVien(collectionEmployee, items);
                    LayNhanVien(collectionEmployee);
                    */

                    textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
                    DGNhanVien.ItemsSource = employeeListDisplay;
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
            MessageBoxResult result = MessageBox.Show("Xóa nhân viên ?", "Cảnh báo", button, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NhanVien item = ((FrameworkElement)sender).DataContext as NhanVien;

                //comment đến.....
                employeeList.Remove(item);
                employeeListDisplay.Remove(item);
                DGNhanVien.ItemsSource = employeeListDisplay;
                //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db

                /* Xóa dữ liệu trong db
                collectionEmployee.DeleteOne(x => x["idNumber"] == item.CCCD);
                LayNhanVien(collectionEmployee);
                */

                textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
                DGNhanVien.ItemsSource = employeeListDisplay;
                autoorder();
                DGNhanVien.Items.Refresh();

            }
        }

        private void searchbox_textchanged(object sender, TextChangedEventArgs e)
        {
            employeeListDisplay.Clear();
            int count = employeeList.Count();
            string text = searchbox.Text;
            foreach (NhanVien P in employeeList)
            {
                if (P.TenNV.Contains(text))
                {
                    employeeListDisplay.Add(P);
                }
            }
            autoorder();
            DGNhanVien.Items.Refresh();
        }

        private void EditStaff_Click(object sender, RoutedEventArgs e)
        {
            bordersuanhanvien.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;

        }
        private void AddStaff_Click(object sender, RoutedEventArgs e)
        {
            borderthemnhanvien.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            bordersuanhanvien.Visibility = Visibility.Collapsed;
            borderthemnhanvien.Visibility = Visibility.Collapsed;
            Dialog.IsOpen = false;
        }

        private void DGHoadonnhanvien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("navigate tới hóa đơn chi tiết");
        }

    }
}
