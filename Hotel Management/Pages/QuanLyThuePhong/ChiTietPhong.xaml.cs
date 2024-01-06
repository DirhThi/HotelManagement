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
using Hotel_Management.MongoDatabase;
using MongoDB.Driver;
using MongoDB.Bson;
using Amazon.Runtime.SharedInterfaces;
using System.Runtime.CompilerServices;
using System.Windows.Forms.DataVisualization.Charting;

namespace Hotel_Management.Pages.QuanLyThuePhong
{
    /// <summary>
    /// Interaction logic for ChiTietPhong.xaml
    /// </summary>
    public partial class ChiTietPhong : Page
    {
        public static event Action OnNewGuestAdded;
        MongoHandler handler = MongoHandler.GetInstance();
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
            DatePicker2.SelectedDate = DateTime.Now.AddDays(1);
            //FutureDatePicker.BlackoutDates.AddDatesInPast();
            //DatePicker1.BlackoutDates.AddDatesInPast();
            //DatePicker2.BlackoutDates.AddDatesInPast();
            //DatePicker2.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));
            serviceIC.ItemsSource = ServiceList;
            serviceusedDG.ItemsSource = serviceUsedList;
            maphongtb.Text = maphong;
            loaiphongtb.Text = loaiphong;
            ValidateRoomInfomation();
        }   
        public void ValidateRoomInfomation()
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Room");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument room in documents)
                {
                    string roomNumber = room["roomName"].AsString;
                    if (roomNumber==maphongtb.Text)
                    {
                        if (room["roomState"]=="Đang thuê" || room["roomState"]=="Đã đặt")
                        {
                            UserInfo.IsEnabled = false;
                            roomInfo.IsEnabled = false;
                            UpdateFunctionButton(room["roomState"].AsString);
                            BsonDocument receipt = GetCurrentReceipt(room["receiptId"].AsObjectId);
                            GetCurrentGuestInfo(receipt["customerId"].AsObjectId);
                            GetRoomInfo(room["receiptId"].AsObjectId);
                            GetCurrentServiceInfo(receipt["serviceId"].AsBsonArray);

                        }
                        else
                        {
                            FutureDatePicker.BlackoutDates.AddDatesInPast();
                            DatePicker1.BlackoutDates.AddDatesInPast();
                            DatePicker2.BlackoutDates.AddDatesInPast();
                            DatePicker2.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));
                        }
                    }
                }
            }
        }


        public BsonDocument GetCurrentReceipt(ObjectId ObjectID)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Receipt");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    ObjectId receipt = document["_id"].AsObjectId;
                    if (receipt == ObjectID)
                    {        
                        
                        return document;
                    }
                }
            }
            return null;
        }
        public BsonDocument GetService(ObjectId objectId)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    ObjectId receipt = document["_id"].AsObjectId;
                    if (receipt == objectId)
                    {
                        return document;
                    }
                }
            }
            return null;
        }
        public void GetRoomInfo(ObjectId ObjectID)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Receipt");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    ObjectId receipt = document["_id"].AsObjectId;
                    if (receipt == ObjectID)
                    {
                        CreatedDate.Text = document["createDate"].AsDateTime.ToString();
                        RoomName.Text = maphongtb.Text + " - " + loaiphongtb.Text;
                        BsonDocument user = GetUser(document["userId"].AsBsonArray[0].AsObjectId);
                        if (user != null)
                        {
                            Receiptionist.Text = user["userName"].AsString;
                        }
                        CustomerNameOnReceipt.Text = CustomerName.Text;
                        ServiceType.Text = document["receiptType"].AsString;
                        DateReceived.Text = document["checkIn"].AsDateTime.ToString();
                        DateReturned.Text = document["checkOut"].AsDateTime.ToString();
                        if (document["receiptType"].AsString == "Theo ngày")
                        {
                            ButtonNgayCheck();
                            DatePicker1.SelectedDate = document["checkIn"].AsDateTime;
                            DatePicker2.SelectedDate = document["checkOut"].AsDateTime;
                        }
                        else

                            if (document["receiptType"].AsString == "Theo giờ")
                        {
                            ButtonGioCheck();
                            FutureDatePicker.SelectedDate = document["checkIn"].AsDateTime;
                            PresetTimePicker.SelectedTime = document["checkIn"].AsDateTime;
                            TimeSpan TotalUse = document["checkOut"].AsDateTime.Subtract(document["checkIn"].AsDateTime);
                            giosudung.Text = TotalUse.TotalHours.ToString();
                        }
                        else
                        if (document["receiptType"].AsString == "Qua đêm")
                        {
                            ButtonDemCheck();
                            DatePicker3.SelectedDate = document["checkIn"].AsDateTime;
                        }
                        totalRoomPrice = document["roomCost"].AsInt32;
                        RoomCost.Text = document["roomCost"].AsInt32.ToString();
                    }
                }
            }
        }

        public void ButtonNgayCheck()
        {
            radiobtnngay.IsChecked = true;
            bordergio.Visibility = Visibility.Hidden;
            borderngay.Visibility = Visibility.Visible;
            borderdem.Visibility = Visibility.Hidden;

        }
        public void ButtonGioCheck()
        {
            radiobtngio.IsChecked = true;
            bordergio.Visibility = Visibility.Visible;
            borderngay.Visibility = Visibility.Hidden;
            borderdem.Visibility = Visibility.Hidden;

        }
        public void ButtonDemCheck()
        {
            radiobtndem.IsChecked = true;
            bordergio.Visibility = Visibility.Hidden;
            borderngay.Visibility = Visibility.Hidden;
            borderdem.Visibility = Visibility.Visible;

        }

        public BsonDocument GetUser(ObjectId UserId)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("User");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    ObjectId user = document["_id"].AsObjectId;
                    if (user == UserId)
                    {
                        return document;
                    }
                }
            }
            return null;
        }
        public void GetCurrentGuestInfo(ObjectId ObjectID)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Customer");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    ObjectId id = document["_id"].AsObjectId;
                    if (id == ObjectID)
                    {
                        CustomerName.Text = document["customerName"].AsString;
                        CustomerBirth.Text = document["dateOfBirth"].AsDateTime.ToString();
                        CustomerPhoneNumber.Text = document["phoneNumber"].AsString;
                        CustomerEmail.Text = document["email"].AsString;
                        CustomerIdNumber.Text = document["idNumber"].AsString;
                    }
                }
            }
        }

        public void GetCurrentServiceInfo(BsonArray serviceList)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("ServiceUsed");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonValue service in serviceList)
                {
                    foreach (BsonDocument document in documents)
                    {
                        if (service.AsObjectId == document["_id"].AsObjectId)
                        {
                            BsonDocument Service = GetService(document["serviceId"].AsObjectId);
                            UpdateService(Service["serviceName"].AsString, document["serviceQuantity"].AsInt32);
                        }
                    }
                }
            }
        }
        public void UpdateFunctionButton(string roomState)
        {
            switch (roomState)
            {
                case "Đang dọn dẹp":
                case "Trống":
                    phongtrongoption.Visibility = Visibility.Visible;
                    phongdatoption.Visibility = Visibility.Collapsed;
                    phongthueoption.Visibility = Visibility.Collapsed;
                    break;
                case "Đang thuê":
                    phongtrongoption.Visibility = Visibility.Collapsed;
                    phongdatoption.Visibility = Visibility.Collapsed;
                    phongthueoption.Visibility = Visibility.Visible;
                    break;
                case "Đã đặt":
                    phongtrongoption.Visibility = Visibility.Collapsed;
                    phongdatoption.Visibility = Visibility.Visible;
                    phongthueoption.Visibility = Visibility.Collapsed;
                    break;
                

            }
        }

        public void UpdateService(string serviceName, int quantity)
        {
            foreach (service service in  ServiceList)
            {
                if(service.name==serviceName)
                {
                    service.quantity = quantity;
                }
            }
            updateServiceUsed();
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

        private void AddCustomer()
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
                        return;
                    }
                }
                var newDoc = new BsonDocument
                {
                    {"customerName", CustomerName.Text},
                    {"phoneNumber", CustomerPhoneNumber.Text },
                    { "idNumber", CustomerIdNumber.Text},
                    {"dateOfBirth", CustomerBirth.Text},
                    {"email", CustomerEmail.Text},
               
                };
                collection.InsertOne(newDoc);
                StaticEvents.StaticEventHandler.OnCustomerUpdatedEvent();
            }
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
