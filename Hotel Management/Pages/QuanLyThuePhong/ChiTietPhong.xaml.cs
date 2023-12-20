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
using System.ComponentModel;


namespace Hotel_Management.Pages.QuanLyThuePhong
{
    /// <summary>
    /// Interaction logic for ChiTietPhong.xaml
    /// </summary>
    public partial class ChiTietPhong : Page
    {
        List<int> numberList = new List<int>() { 101, 102, 103, 201, 203, 301, 304, 401, 402, 403 };
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

       
        public ChiTietPhong(string maphong, string loaiphong, string trangthai,string loaithue)
        {
            InitializeComponent();
            totalBill = totalRoomPrice + totalServiceUsedPrice;
            totalbilltext.Text = totalBill.ToString();
            FutureDatePicker.BlackoutDates.AddDatesInPast();
            DatePicker1.BlackoutDates.AddDatesInPast();
            DatePicker2.SelectedDate = DateTime.Now.AddDays(1);
            DatePicker2.BlackoutDates.AddDatesInPast();
            DatePicker2.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));
            serviceIC.ItemsSource = ServiceList;
            serviceusedDG.ItemsSource = serviceUsedList;
            maphongtb.Text = maphong;
            loaiphongtb.Text = loaiphong;
            if(trangthai=="phongtrong")
            {
                phongtrongoption.Visibility = Visibility.Visible;
            }
            if (trangthai == "phongthue")
            {
                phongthueoption.Visibility = Visibility.Visible;
            }
            if (trangthai == "phongdat")
            {
                phongdatoption.Visibility = Visibility.Visible;
            }
        }
      
        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            chitietphong.NavigationService.GoBack();
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
            if (gio >1 )
            {
                gio--;
                string gText = gio.ToString();
                giosudung.Text = gText;
            }
        }

        private void giocheck(object sender, RoutedEventArgs e)
        {
            if(radiobtngio.IsChecked==true)
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
            if(serviceUsedList.Count()>0)
            {
                serviceUsedList.Add(new serviceUsed() { stt = "Tổng", nameServiceUsed ="" , price = "", soluong = serviceUsedList.Sum(l=>l.soluong), total = serviceUsedList.Sum(l => l.total) });
                totalServiceUsedPrice = serviceUsedList.Last().total;
            }
            else
            {
                serviceUsedList.Add(new serviceUsed() { stt = "Tổng", nameServiceUsed = "", price = "", soluong = 0, total = 0});
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
    }
    public  class service 
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
