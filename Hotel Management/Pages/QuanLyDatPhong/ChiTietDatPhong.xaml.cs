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
using Hotel_Management.MongoDatabase;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Hotel_Management.Pages.QuanLyDatPhong
{
    /// <summary>
    /// Interaction logic for ChiTietDatPhong.xaml
    /// </summary>
    public partial class ChiTietDatPhong : Page
    {
        MongoHandler handler = MongoHandler.GetInstance();
        List<service> ServiceList = new List<service> {
              new service {name="Nước suối",price=10000,quantity=0,imagesource="/Assets/Images/aquafina.png"},
              new service {name="Sting",price=15000,quantity=0,imagesource="/Assets/Images/sting.jpg"},
              new service {name="Cocacola",price=15000,quantity=0,imagesource="/Assets/Images/cocacola.png"},
              new service {name="Redbull",price=20000,quantity=0,imagesource="/Assets/Images/redbull.png"},
              new service {name="Mì ly",price=15000,quantity=0,imagesource="/Assets/Images/mily.png"},
              new service {name="Bún bò",price=40000,quantity=0,imagesource="/Assets/Images/bunbo.png"},
              new service {name="Phở bò",price=40000,quantity=0,imagesource="/Assets/Images/phobo.png"},

            };
        List<serviceUsed> serviceUsedList = new List<serviceUsed> {
            new serviceUsed() { stt = "Tổng", nameServiceUsed = "", price = "", soluong = 0, total = 0}
        };
        int totalServiceUsedPrice = 0;
        int totalRoomPrice = 100000;
        int totalBill;
        public ChiTietDatPhong(string trangthai)
        {
            InitializeComponent();
            totalBill = totalRoomPrice + totalServiceUsedPrice;
            totalbilltext.Text = totalBill.ToString();
            DatePicker2.SelectedDate = DateTime.Now.AddDays(1);
            serviceIC.ItemsSource = ServiceList;
            serviceusedDG.ItemsSource = serviceUsedList;
            if(trangthai!="Đặt phòng")
            {
                datphontxt.Text = "Nhận phòng";
                datphongoption.Visibility = Visibility.Collapsed;
                nhanphongoption.Visibility = Visibility.Visible;

            }    
        }

        
        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Chitietdatphong.NavigationService.GoBack();
        }

        private void congbtnclick(object sender, RoutedEventArgs e)
        {
            int gio = Int32.Parse(giosudung.Text);
            if (gio < 8)
            {
                gio++;
                string gText = gio.ToString();
                giosudung.Text = gText;
            }
        }

        private void trubtnclick(object sender, RoutedEventArgs e)
        {
            int gio = Int32.Parse(giosudung.Text);
            if (gio > 1)
            {
                gio--;
                string gText = gio.ToString();
                giosudung.Text = gText;
            }
        }

        private void giocheck(object sender, RoutedEventArgs e)
        {
            if (radiobtngio.IsChecked == true)
            {
                bordergio.Visibility = Visibility.Visible;
                borderngay.Visibility = Visibility.Hidden;
                borderdem.Visibility = Visibility.Hidden;
            }
        }

        private void ngaycheck(object sender, RoutedEventArgs e)
        {
            if (radiobtnngay.IsChecked == true)
            {

                borderngay.Visibility = Visibility.Visible;
                bordergio.Visibility = Visibility.Hidden;
                borderdem.Visibility = Visibility.Hidden;
            }
        }

        private void demcheck(object sender, RoutedEventArgs e)
        {
            if (radiobtndem.IsChecked == true)
            {
                borderdem.Visibility = Visibility.Visible;
                bordergio.Visibility = Visibility.Hidden;
                borderngay.Visibility = Visibility.Hidden;
            }
        }

        private void updateServiceUsed()
        {
            serviceUsedList.Clear();
            int i = 0;
            foreach (service item in ServiceList)
            {
                if (item.quantity > 0)
                {
                    i++;
                    serviceUsedList.Add(new serviceUsed() { stt = i.ToString(), nameServiceUsed = item.name, price = item.price.ToString(), soluong = item.quantity, total = item.price * item.quantity });
                }
            }
            if (serviceUsedList.Count() > 0)
            {
                serviceUsedList.Add(new serviceUsed() { stt = "Tổng", nameServiceUsed = "", price = "", soluong = serviceUsedList.Sum(l => l.soluong), total = serviceUsedList.Sum(l => l.total) });
                totalServiceUsedPrice = serviceUsedList.Last().total;
            }
            else
            {
                serviceUsedList.Add(new serviceUsed() { stt = "Tổng", nameServiceUsed = "", price = "", soluong = 0, total = 0 });
                totalServiceUsedPrice = 0;
            }
            totalBill = totalRoomPrice + totalServiceUsedPrice;
            totalbilltext.Text = totalBill.ToString();
            serviceusedDG.Items.Refresh();
        }

        private void truservicebtn_Click(object sender, RoutedEventArgs e)
        {
            service Service = (sender as Button).DataContext as service;
            if (Service.quantity > 0)
            {
                int temp = Service.quantity;
                temp--;
                Service.quantity = temp;
            }
            serviceIC.Items.Refresh();
            updateServiceUsed();
        }


        private void congservicebtn_Click(object sender, RoutedEventArgs e)
        {
            service Service = (sender as Button).DataContext as service;
            int temp = Service.quantity;
            temp++;
            Service.quantity = temp;
            serviceIC.Items.Refresh();
            updateServiceUsed();
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Customer");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument customer in documents)
                {
                    string idNumber = customer["idNumber"].AsString;
                    if (idNumber == CustomerIdNumber.Text)
                    {
                        CustomerName.Text = customer["customerName"].AsString;
                        CustomerBirth.Text = customer["dateOfBirth"].AsDateTime.ToString();
                        CustomerPhoneNumber.Text = customer["phoneNumber"].AsString;
                        CustomerEmail.Text = customer["email"].AsString;
                    }
                }
            }
        }

        private void CustomerPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Customer");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument customer in documents)
                {
                    string idNumber = customer["phoneNumber"].AsString;
                    if (idNumber == CustomerPhoneNumber.Text)
                    {
                        CustomerName.Text = customer["customerName"].AsString;
                        CustomerBirth.Text = customer["dateOfBirth"].AsDateTime.ToString();
                        CustomerIdNumber.Text = customer["idNumber"].AsString;
                        CustomerEmail.Text = customer["email"].AsString;
                    }
                }
            }
        }

        private void nhanphong_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;

        }

        private void chonphong_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class service
    {
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string imagesource { get; set; }
    }

    public class serviceUsed
    {
        public string stt { get; set; }
        public string nameServiceUsed { get; set; }
        public string price { get; set; }
        public int soluong { get; set; }
        public int total { get; set; }

    }
}
