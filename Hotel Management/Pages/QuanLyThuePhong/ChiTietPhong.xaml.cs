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
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Diagnostics;

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
        List<service> ServiceList = new List<service>();
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
            Receiptionist.Text = Pages.Auth.Login.currentUser.UserName;
            RoomName.Text = maphongtb.Text + " - " + loaiphongtb.Text;
            DatePicker1.SelectedDate = DateTime.Now;
            DatePicker2.SelectedDate = DateTime.Now.AddDays(1);
            DatePicker3.SelectedDate = DateTime.Now;
            FutureDatePicker.SelectedDate = DateTime.Now;
            PresetTimePicker.SelectedTime = DateTime.Now;
            UpdateRoomPricePreset();
            UpdateServiceInfo();
            ValidateRoomInfomation();
            UpdateTotalCost();
        }   

        void UpdateRoomPricePreset()
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("RoomType");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    if (loaiphongtb.Text == document["roomType"].AsString)
                    {
                        dayPrice.Text = (document["dayPrice"].AsInt32 / 1000).ToString()+"K/ngày";
                        nightPrice.Text = (document["nightPrice"].AsInt32 / 1000).ToString() + "K/đêm";
                        hourPrice.Text = (document["firstHourPrice"].AsInt32/1000).ToString() + "K + " + (document["followupHourPrice"].AsInt32/1000).ToString() +"K/H";
                        return;
                    }
                }
            }
        }
        void UpdateTotalCost()
        {
            totalbilltext.Text = Convert.ToString(Convert.ToInt32(RoomCost.Text) + totalServiceUsedPrice);
        }
        private void UpdateServiceInfo()
        {
            if (handler != null)
            {
                ServiceList.Clear();
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument service in documents)
                {
                    string imageSource;
                    if (service["serviceImage"].AsString == "")
                    {
                        imageSource = "/Assets/Images/addImage.png";
                    }
                    else
                    {
                        imageSource = service["serviceImage"].AsString;
                    }
                    ServiceList.Add(new service { name = service["serviceName"].AsString, price = service["servicePrice"].AsInt32, quantity = 0, imagesource = imageSource });
                }
                serviceIC.Items.Refresh();
            }
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
                        if (!document["dateOfBirth"].IsBsonNull)
                        {
                            CustomerBirth.Text = document["dateOfBirth"].AsDateTime.ToString();
                        } 
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
                case "Trống":
                    phongtrongoption.Visibility = Visibility.Visible;
                    phongdatoption.Visibility = Visibility.Collapsed;
                    phongthueoption.Visibility = Visibility.Collapsed;
                    hoadonoption.Visibility = Visibility.Collapsed;
                    break;
                case "Đang thuê":
                    phongtrongoption.Visibility = Visibility.Collapsed;
                    phongdatoption.Visibility = Visibility.Collapsed;
                    phongthueoption.Visibility = Visibility.Visible;
                    hoadonoption.Visibility = Visibility.Collapsed;
                    break;
                case "Đã đặt":
                    phongtrongoption.Visibility = Visibility.Collapsed;
                    phongdatoption.Visibility = Visibility.Visible;
                    phongthueoption.Visibility = Visibility.Collapsed;
                    hoadonoption.Visibility = Visibility.Collapsed;
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
                ServiceType.Text = "Theo giờ";
                UpdateRoomPrice();
            }

        }

        private void ngaycheck(object sender, RoutedEventArgs e)
        {
            if (radiobtnngay.IsChecked == true)
            {

                borderngay.Visibility = Visibility.Visible;
                bordergio.Visibility = Visibility.Hidden;
                borderdem.Visibility = Visibility.Hidden;
                ServiceType.Text = "Theo ngày";
                UpdateRoomPrice();
            }
        }

        private void demcheck(object sender, RoutedEventArgs e)
        {
            if (radiobtndem.IsChecked == true)
            {
                borderdem.Visibility = Visibility.Visible;
                bordergio.Visibility = Visibility.Hidden;
                borderngay.Visibility = Visibility.Hidden;
                ServiceType.Text = "Qua đêm";
                UpdateRoomPrice();
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
            UpdateTotalCost();
            serviceusedDG.Items.Refresh();
        }

        private void truservicebtn_Click(object sender, RoutedEventArgs e)
        {
            service Service = (sender as System.Windows.Controls.Button).DataContext as service;
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
            service Service = (sender as System.Windows.Controls.Button).DataContext as service;
            int temp = Service.quantity;
            temp++;
            Service.quantity = temp;
            serviceIC.Items.Refresh();
            updateServiceUsed();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void CustomerPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }


        private BsonDocument AddCustomer()
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
                        return customer;
                    }
                }
                BsonDocument newDoc;
                if (String.IsNullOrEmpty(CustomerBirth.Text))
                {
                    newDoc = new BsonDocument
                    {
                        {"customerName", CustomerName.Text},
                        {"phoneNumber", CustomerPhoneNumber.Text },
                        { "idNumber", CustomerIdNumber.Text},
                        {"dateOfBirth", BsonNull.Value },
                        {"email", CustomerEmail.Text},

                    };
                }
                else
                {

                    newDoc = new BsonDocument
                    {
                        {"customerName", CustomerName.Text},
                        {"phoneNumber", CustomerPhoneNumber.Text },
                        { "idNumber", CustomerIdNumber.Text},
                        {"dateOfBirth", Convert.ToDateTime(CustomerBirth.Text)},
                        {"email", CustomerEmail.Text},

                    };

                }

                collection.InsertOne(newDoc);
                StaticEvents.StaticEventHandler.OnCustomerUpdatedEvent();
                return newDoc;              
            }
            else
            { return null; }
        }

        private void CustomerName_LostFocus(object sender, RoutedEventArgs e)
        {
            CustomerNameOnReceipt.Text = CustomerName.Text;
        }

        private void FutureDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (radiobtngio.IsChecked == true)
            {
                if (DateReceived != null)
                {
                    DateTime dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    dateTime = dateTime.AddHours(Convert.ToDouble(giosudung.Text));
                    DateReturned.Text = dateTime.ToString();
                    UpdateRoomPrice();
                }
            }
        }

        private void DatePicker1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (radiobtnngay.IsChecked == true)
            {
                DateTime dateTime = DatePicker1.SelectedDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                DateReceived.Text = dateTime.ToString();
                if(DatePicker2.SelectedDate<DatePicker1.SelectedDate)
                {
                    dateTime = dateTime.AddDays(1);
                    DatePicker2.SelectedDate = dateTime;
                    DateReturned.Text = dateTime.ToString();
                }
                else
                {
                    dateTime = DatePicker2.SelectedDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                    DateReturned.Text = dateTime.ToString();
                }
                UpdateRoomPrice();
            }
        }

        private void DatePicker2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (radiobtnngay.IsChecked == true)
            {
                DateTime dateTime = DatePicker1.SelectedDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                DateReceived.Text = dateTime.ToString();
                if (DatePicker2.SelectedDate < DatePicker1.SelectedDate)
                {
                    dateTime = dateTime.AddDays(1);
                    DatePicker2.SelectedDate = dateTime;
                    DateReturned.Text = dateTime.ToString();
                }
                else
                {
                    dateTime = DatePicker2.SelectedDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                    DateReturned.Text = dateTime.ToString();
                }
                UpdateRoomPrice();
            }
        }

        private void DatePicker3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (radiobtndem.IsChecked == true)
            {
                DateReceived.Text = DateTime.Now.ToString();
                DateTime dateTime = DatePicker3.SelectedDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                DateReturned.Text = dateTime.ToString();
                UpdateRoomPrice();
            }
        }

        private void PresetTimePicker_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void giosudung_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (radiobtngio.IsChecked == true)
            {
                if (DateReceived != null)
                {
                    DateTime dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    dateTime = dateTime.AddHours(Convert.ToDouble(giosudung.Text));
                    DateReturned.Text = dateTime.ToString();
                    UpdateRoomPrice();
                }
            }

        }

        private void PresetTimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (radiobtngio.IsChecked == true)
            {              
                if (DateReceived != null)
                {
                    DateTime dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    DateReceived.Text = dateTime.ToString();
                    dateTime = FutureDatePicker.SelectedDate.Value.Date.Add(PresetTimePicker.SelectedTime.Value.TimeOfDay);
                    dateTime = dateTime.AddHours(Convert.ToDouble(giosudung.Text));
                    DateReturned.Text = dateTime.ToString();
                    UpdateRoomPrice();
                }
            }
        }

        void UpdateRoomPrice()
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("RoomType");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    if (loaiphongtb.Text == document["roomType"].AsString)
                    {
                        if (ServiceType.Text == "Theo ngày")
                        {
                            TimeSpan day = DatePicker2.SelectedDate.Value.Date.Subtract(DatePicker1.SelectedDate.Value.Date);
                            RoomCost.Text = Convert.ToString(document["dayPrice"].AsInt32 * day.Days);
                        }
                        else
                        if (ServiceType.Text == "Theo giờ")
                        {
                            if(Convert.ToInt32(giosudung.Text)>1)
                            {
                                int cost = document["firstHourPrice"].AsInt32;
                                cost += ((Convert.ToInt32(giosudung.Text) - 1) * document["followupHourPrice"].AsInt32);
                                RoomCost.Text = cost.ToString();
                            }
                            else
                            {
                                RoomCost.Text = document["firstHourPrice"].AsInt32.ToString();
                            }

                        }
                        else
                        if (ServiceType.Text == "Qua đêm")
                        {
                            RoomCost.Text = document["nightPrice"].AsInt32.ToString();
                        }
                        UpdateTotalCost();
                        return;
                    }
                }
            }
        }

        BsonDocument GetCurrentRoomDocument(string roomNumber)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Room");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    if (roomNumber == document["roomName"].AsString)
                    {
                        return document;
                    }
                }
                return null;
            }
            else
            { return null; }
        }

        BsonDocument GetCurrentUserDocuement(Pages.Auth.User currentUser)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("User");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument document in documents)
                {
                    if (currentUser.IdNumber == document["idNumber"].AsString)
                    {
                        return document;
                    }
                }
                return null;
            }
            else
            { return null; }
        }

        BsonDocument GetServiceDocument(string serviceName)
        {
            if (handler != null)
            {
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach(BsonDocument document in documents)
                {
                    if (serviceName == document["serviceName"].AsString)
                    {
                        return document;
                    }
                }
                return null;
            }
            else
            { return null; }
        }

        BsonArray GetServiceUsedList()
        {
            BsonArray bsonArray = new BsonArray();
            foreach (service service in ServiceList)
            {
                BsonDocument serviceName = GetServiceDocument(service.name);
                if(serviceName != null && service.quantity>0)
                {
                    IMongoCollection<BsonDocument> serviceUsed = handler.GetCollection("ServiceUsed");
                    var newDoc = new BsonDocument
                    {
                        {"serviceId", serviceName["_id"].AsObjectId },
                        {"serviceQuantity", service.quantity },
                        {"pricePerUnit", service.price }
                    };
                    serviceUsed.InsertOne(newDoc);
                    bsonArray.Add(newDoc["_id"].AsObjectId);
                }
            }
            return bsonArray;
        }

        void AddReceiptToUser(BsonDocument receipt, BsonDocument user)
        {
            IMongoCollection<BsonDocument> collection = handler.GetCollection("User");
            var filter = Builders<BsonDocument>.Filter.Eq(r => r["_id"], user["_id"].AsObjectId);
            var update = Builders<BsonDocument>.Update.AddToSet(r => r["receiptId"].AsBsonArray, receipt["_id"].AsObjectId);
            collection.UpdateOne(filter, update);
        }
        BsonDocument AddNewReceipt(BsonDocument customer, BsonArray serviceUsed, BsonDocument currentUser, BsonDocument currentRoom)
        {
            if (handler != null)
            {
                if (customer != null && serviceUsed != null && currentUser != null)
                {
                    IMongoCollection<BsonDocument> collection = handler.GetCollection("Receipt");
                    List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                    int highestIdCode = 0;
                    foreach(BsonDocument document in documents)
                    {
                        if (highestIdCode < Convert.ToInt32(document["idCode"].AsString))
                        {
                            highestIdCode = Convert.ToInt32(document["idCode"].AsString);
                        }

                    }
                    var newDoc = new BsonDocument
                    {
                        {"idCode", StaticClass.StringHandler.GenerateIdCode(highestIdCode+1) },
                        {"userId", new BsonArray{ currentUser["_id"].AsObjectId } },
                        {"createDate", Convert.ToDateTime(CreatedDate.Text) },
                        { "roomId", currentRoom["_id"].AsObjectId},
                        {"receiptState", "Chưa thanh toán"},
                        {"customerId", customer["_id"].AsObjectId},
                        {"receiptType", ServiceType.Text },
                        {"checkIn", Convert.ToDateTime(DateReceived.Text) },
                        {"checkOut", Convert.ToDateTime(DateReturned.Text) },
                        {"roomCost", Convert.ToInt32(RoomCost.Text) },
                        {"serviceId", serviceUsed },
                        {"receiptTotal", Convert.ToInt32(totalbilltext.Text) },
                        {"roomType", loaiphongtb.Text},

                    };
                    collection.InsertOne(newDoc);
                    IMongoCollection<BsonDocument> roomCollection = handler.GetCollection("Room");
                    var filter = Builders<BsonDocument>.Filter.Eq(r => r["roomName"], currentRoom["roomName"].AsString);
                    var update = Builders<BsonDocument>.Update.Set(r => r["roomState"], "Đang thuê");
                    roomCollection.UpdateOne(filter, update);
                    update = Builders<BsonDocument>.Update.Set(r => r["receiptId"], newDoc["_id"].AsObjectId);
                    roomCollection.UpdateOne(filter, update);
                    return newDoc;
                }
                else
                    return null;
            }
            else
            { return null; }
        }

        private void phongtrong_datphong_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phongtrong_nhanphong_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(CustomerName.Text) || String.IsNullOrEmpty(CustomerIdNumber.Text) || String.IsNullOrEmpty(CustomerPhoneNumber.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cần thiết.");
                return;
            }
            else
             if (!StaticClass.StringHandler.IsDateTime(CustomerBirth.Text))
            {
                MessageBox.Show("Vui nhập đúng định dạng ngày sinh DD/MM/YYYY.");
                return;
            }
            else

            {
                BsonDocument newCustomer = AddCustomer();
                BsonDocument receipt = AddNewReceipt(newCustomer, GetServiceUsedList(), GetCurrentUserDocuement(Pages.Auth.Login.currentUser), GetCurrentRoomDocument(maphongtb.Text));
                AddReceiptToUser(receipt, GetCurrentUserDocuement(Pages.Auth.Login.currentUser));
                MessageBox.Show("Nhận phòng thành công, vui lòng giao chìa cho khách và nhắc nhở thanh toán khi trả phòng");
                chitietphong.NavigationService.GoBack();
            }
        }

        private void phongdat_huyphong_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phongdat_nhanphong_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phongthue_xacnhan_Click(object sender, RoutedEventArgs e)
        {
            IMongoCollection<BsonDocument> receiptCollection = handler.GetCollection("Receipt");

            var filter = Builders<BsonDocument>.Filter.Eq(r => r["_id"], GetCurrentRoomDocument(maphongtb.Text)["receiptId"].AsObjectId);
            var update = Builders<BsonDocument>.Update.Set(r => r["serviceId"], GetServiceUsedList());
            receiptCollection.UpdateOne(filter, update);
            update = Builders<BsonDocument>.Update.Set(r => r["receiptTotal"], Convert.ToInt32(totalbilltext.Text));
            receiptCollection.UpdateOne(filter, update);
            chitietphong.NavigationService.GoBack();
        }

        private void phongthue_traphong_Click(object sender, RoutedEventArgs e)
        {
            IMongoCollection<BsonDocument> receiptCollection = handler.GetCollection("Receipt");
            var filter = Builders<BsonDocument>.Filter.Eq(r => r["_id"], GetCurrentRoomDocument(maphongtb.Text)["receiptId"].AsObjectId);
            var update = Builders<BsonDocument>.Update.Set(r => r["receiptState"], "Đã thanh toán");
            receiptCollection.UpdateOne(filter, update);

            IMongoCollection<BsonDocument> roomCollection = handler.GetCollection("Room");
            filter = Builders<BsonDocument>.Filter.Eq(r => r["roomName"],maphongtb.Text);
            update = Builders<BsonDocument>.Update.Set(r => r["roomState"], "Đang dọn dẹp");
            roomCollection.UpdateOne(filter, update);
            update = Builders<BsonDocument>.Update.Set(r => r["receiptId"], BsonNull.Value);
            roomCollection.UpdateOne(filter, update);
            MessageBox.Show("Trả phòng thành công","Lưu ý");
            chitietphong.NavigationService.GoBack();
        }

        private void CustomerIdNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !StaticClass.StringHandler.IsTextAllowed(e.Text);
        }

        private void CustomerPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !StaticClass.StringHandler.IsTextAllowed(e.Text);
        }

        private void CustomerIdNumber_LostFocus(object sender, RoutedEventArgs e)
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
                        if (!customer["dateOfBirth"].IsBsonNull)
                        {
                            CustomerBirth.Text = customer["dateOfBirth"].AsDateTime.ToString();
                        }
                        if (!customer["email"].IsBsonNull)
                        {
                            CustomerEmail.Text = customer["email"].AsString;
                        }
                        CustomerPhoneNumber.Text = customer["phoneNumber"].AsString;                     
                    }
                }
            }
        }

        private void CustomerPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
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
                        if (!customer["dateOfBirth"].IsBsonNull)
                        {
                            CustomerBirth.Text = customer["dateOfBirth"].AsDateTime.ToString();
                        }
                        if (!customer["email"].IsBsonNull)
                        {
                            CustomerEmail.Text = customer["email"].AsString;
                        }
                        CustomerIdNumber.Text = customer["idNumber"].AsString;
                    }
                }
            }
        }

        private void CustomerName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !StaticClass.StringHandler.IsNumberAndSymbolAllowed(e.Text);
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
