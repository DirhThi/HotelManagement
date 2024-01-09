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
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.ObjectModel;
using MongoDB.Driver.Linq;
using System.Collections;

namespace Hotel_Management.Pages.QuanLyDatPhong
{
    /// <summary>
    /// Interaction logic for ChiTietDatPhong.xaml
    /// </summary>
    public partial class ChiTietDatPhong : Page
    {
        static MongoHandler handler = MongoHandler.GetInstance();
        IMongoCollection<BsonDocument> collectionCustomer = handler.GetCollection("Customer");
        IMongoCollection<BsonDocument> collectionRoom = handler.GetCollection("Room");
        IMongoCollection<BsonDocument> collectionRoomType = handler.GetCollection("RoomType");
        IMongoCollection<BsonDocument> collectionReceipt = handler.GetCollection("Receipt");
        IMongoCollection<BsonDocument> collectionUser = handler.GetCollection("User");
        IMongoCollection<BsonDocument> collectionServiceUsed = handler.GetCollection("ServiceUsed");
        IMongoCollection<BsonDocument> collectionService = handler.GetCollection("Service");
        List<service> ServiceList = new List<service> {
              new service {name="Nước suối",price=10000,quantity=0,imagesource="/Assets/Images/aquafina.png"},
              new service {name="Sting",price=15000,quantity=0,imagesource="/Assets/Images/sting.jpg"},
              new service {name="Cocacola",price=15000,quantity=0,imagesource="/Assets/Images/cocacola.png"},
              new service {name="Redbull",price=20000,quantity=0,imagesource="/Assets/Images/redbull.png"},
              new service {name="Mì ly",price=15000,quantity=0,imagesource="/Assets/Images/mily.png"},
              new service {name="Bún bò",price=40000,quantity=0,imagesource="/Assets/Images/bunbo.png"},
              new service {name="Phở bò",price=40000,quantity=0,imagesource="/Assets/Images/phobo.png"},

            };
        List<serviceUsed> serviceUsedList = new List<serviceUsed> ();
        int totalServiceUsedPrice = 0;
        int totalRoomPrice = 100000;
        int totalBill;
        Bill billBook = new Bill();
        static DateTime checkOut = new DateTime();
        List<string> roomTypeList = new List<string>();
        int tongDichVu = 0;
        List<string> phongTrong = new List<string>();
        public ChiTietDatPhong(string trangthai, Bill item = null)
        {
            InitializeComponent();
            totalBill = totalRoomPrice + totalServiceUsedPrice;
            totalbilltext.Text = totalBill.ToString();
            LayDichVu(collectionService);
            serviceIC.ItemsSource = ServiceList;
            serviceusedDG.ItemsSource = serviceUsedList;
            DatePicker2.SelectedDate = DatePicker1.SelectedDate.Value.AddDays(1);
            if(trangthai!="Đặt phòng")
            {
                datphontxt.Text = "Nhận phòng";
                datphongoption.Visibility = Visibility.Collapsed;
                nhanphongoption.Visibility = Visibility.Visible;
                if (item != null)
                {
                    LayListLoaiPhong(collectionRoomType);
                    roomOption.Items.Clear();
                    roomOption.ItemsSource = roomTypeList;
                    LayKhachHangDat(collectionReceipt, collectionCustomer, item);
                    LayPhongDat(collectionReceipt, collectionCustomer, item);
                    LayHoaDonDat(collectionReceipt, collectionServiceUsed, item);
                    LayPhongTrongDat();
                    roomNumberOption.Items.Clear();
                    roomNumberOption.ItemsSource = phongTrong;
                    billBook = item;
                }
            }    
        }
        
        /*Truy xuất hóa đơn đã đặt*/
        public void LayListLoaiPhong(IMongoCollection<BsonDocument> collectionRoomType)
        {
            List<BsonDocument> documentRoomType = collectionRoomType.Find(new BsonDocument()).ToList();
            roomTypeList.Clear();
            foreach (BsonDocument room in documentRoomType)
            {
                roomTypeList.Add(room["roomType"].AsString);
            }
        }

        public void LayKhachHangDat(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, Bill item)
        {
            var rFilter = Builders<BsonDocument>.Filter.Eq("idCode", item.ID);
            BsonDocument documentBill = collectionReceipt.Find(rFilter).FirstOrDefault();
            if (documentBill != null)
            {
                var cFilter = Builders<BsonDocument>.Filter.Where(x => x["_id"].AsObjectId == documentBill["customerId"].AsObjectId);
                BsonDocument documentCustomer = collectionCustomer.Find(cFilter).FirstOrDefault();
                if (documentCustomer != null)
                {
                    CustomerName.Text = documentCustomer["customerName"].AsString;
                    CustomerNameOnReceipt.Text = CustomerName.Text;
                    CustomerIdNumber.Text = documentCustomer["idNumber"].AsString;
                    CustomerPhoneNumber.Text = documentCustomer["phoneNumber"].AsString;
                    CustomerEmail.Text = documentCustomer["email"].AsString;
                    CustomerBirth.Text = documentCustomer["dateOfBirth"].ToLocalTime().ToShortDateString();
                    CustomerName.Focusable = false;
                    CustomerIdNumber.Focusable = false;
                    CustomerEmail.Focusable = false;
                    CustomerBirth.Focusable = false;
                    CustomerPhoneNumber.Focusable = false;
                }
            }
        }

        public void LayPhongDat(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, Bill item)
        {
            var rFilter = Builders<BsonDocument>.Filter.Eq("idCode", item.ID);
            BsonDocument documentBill = collectionReceipt.Find(rFilter).FirstOrDefault();
            if (documentBill != null)
            {
                var rmFilter = Builders<BsonDocument>.Filter.Where(x => x["roomType"].AsString == documentBill["roomType"].AsString && x["roomState"].AsString == "Trống");
                List<BsonDocument> documentRoom = collectionRoom.Find(rmFilter).ToList();
                switch (item.LoaiThue)
                {
                    case "Theo giờ":
                        int gioThue = (documentBill["checkOut"].ToUniversalTime() - documentBill["checkIn"].ToUniversalTime()).Hours;
                        giosudung.Text = gioThue.ToString();
                        checkOut = System.DateTime.Now.AddHours(gioThue);
                        radiobtngio.IsChecked = true;
                        bordergio.Visibility = Visibility.Visible;
                        borderngay.Visibility = Visibility.Hidden;
                        borderdem.Visibility = Visibility.Hidden;
                        break;
                    case "Theo ngày":
                        int ngayThue = (documentBill["checkOut"].ToUniversalTime() - documentBill["checkIn"].ToUniversalTime()).Days;
                        DatePicker2.SelectedDate = DatePicker1.SelectedDate.Value.AddDays(ngayThue);
                        checkOut = System.DateTime.Now.AddDays(ngayThue);
                        radiobtnngay.IsChecked = true;
                        borderngay.Visibility = Visibility.Visible;
                        bordergio.Visibility = Visibility.Hidden;
                        borderdem.Visibility = Visibility.Hidden;
                        break;
                    case "Qua đêm":
                        int gioThueDem = (documentBill["checkOut"].ToUniversalTime() - documentBill["checkIn"].ToUniversalTime()).Hours;
                        checkOut = System.DateTime.Now.AddHours(gioThueDem);
                        radiobtndem.IsChecked = true;
                        borderdem.Visibility = Visibility.Visible;
                        bordergio.Visibility = Visibility.Hidden;
                        borderngay.Visibility = Visibility.Hidden;
                        break;
                    default:
                        break;
                }

                switch (item.Phong)
                {
                    case "Standard":
                        roomOption.SelectedIndex = 0;
                        textGiaGio.Text = (LayGia(item.Phong, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                        textGiaNgay.Text = (LayGia(item.Phong, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                        textGiaDem.Text = (LayGia(item.Phong, "Qua đêm") / 1000.0).ToString() + "k/đêm";
                        break;
                    case "Deluxe":
                        roomOption.SelectedIndex = 1;
                        textGiaGio.Text = (LayGia(item.Phong, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                        textGiaNgay.Text = (LayGia(item.Phong, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                        textGiaDem.Text = (LayGia(item.Phong, "Qua đêm") / 1000.0).ToString() + "k/đêm";

                        break;
                    case "Vip":
                        roomOption.SelectedIndex = 2;
                        textGiaGio.Text = (LayGia(item.Phong, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                        textGiaNgay.Text = (LayGia(item.Phong, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                        textGiaDem.Text = (LayGia(item.Phong, "Qua đêm") / 1000.0).ToString() + "k/đêm";
                        break;
                    default:
                        break;
                }
            }
            textGiaGioChiTiet.Text = "Giá giờ đầu là " + (LayGia(item.Phong, "Theo giờ") / 1000.0).ToString() + "k/giờ, các giờ sau " + (LayGiaGioSau(item.Phong) / 1000.0).ToString() + "k/giờ.";
            textChiTietNgayVao.Text = "Giờ nhận phòng: " + System.DateTime.Now.ToShortTimeString() + " ngày " + DatePicker1.SelectedDate.Value.ToShortDateString();
            textChiTietNgayDi.Text = "Giờ trả phòng: " + System.DateTime.Now.ToShortTimeString() + " ngày " + DatePicker2.SelectedDate.Value.ToShortDateString();
            textChiTietVaoDem.Text = "Giờ nhận phòng: " + System.DateTime.Now.ToShortTimeString() + " ngày" + DatePicker1.SelectedDate.Value.ToShortDateString();
        }

        public void LayHoaDonDat(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionServiceUsed, Bill item)
        {
            var rFilter = Builders<BsonDocument>.Filter.Eq("idCode", item.ID);
            BsonDocument documentBill = collectionReceipt.Find(rFilter).FirstOrDefault();
            CreatedDate.Text = documentBill["createDate"].ToLocalTime().ToString();
            Receiptionist.Text = item.nameStaff;
            CustomerNameOnReceipt.Text = item.nameCustomer;
            ServiceType.Text = documentBill["receiptType"].AsString;
            DateReceived.Text = System.DateTime.Now.ToString();
            DateReturned.Text = checkOut.ToString();
            RoomCost.Text = documentBill["roomCost"].ToString();
            totalbilltext.Text = documentBill["receiptTotal"].ToString();
            if (documentBill["serviceId"].AsBsonArray.Count > 0)
            {
                serviceUsedList.Clear();
                int serviceQuantity = -1;
                string serviceName = "";
                int servicePrice = -1;
                int serviceTotal = -1;
                int servicesTotal = 0;
                foreach (ObjectId sId in documentBill["serviceId"].AsBsonArray)
                {
                    var sFilter = Builders<BsonDocument>.Filter.Where(a => a["_id"].AsObjectId == sId);
                    List<BsonDocument> documentsServiceUsed = collectionServiceUsed.Find(sFilter).ToList();
                    foreach (BsonDocument service in documentsServiceUsed)
                    {
                        serviceQuantity = service["serviceQuantity"].AsInt32;
                        serviceName = LayServiceName(service["serviceId"].AsObjectId, collectionService);
                        servicePrice = LayServicePrice(service["serviceId"].AsObjectId, collectionService);
                        serviceTotal = serviceQuantity * servicePrice;
                        servicesTotal += serviceTotal;
                    }
                    serviceUsedList.Add(new serviceUsed { nameServiceUsed = serviceName, price = servicePrice.ToString(), soluong = serviceQuantity, total = serviceTotal });
                }
                foreach (serviceUsed su in serviceUsedList)
                {
                    foreach (service s in ServiceList)
                    {
                        if (s.name == su.nameServiceUsed)
                        {
                            s.quantity = su.soluong;
                        }
                    }
                }
                serviceusedDG.ItemsSource = serviceUsedList;
                serviceusedDG.Items.Refresh();
                serviceIC.Items.Refresh();
                tongDichVu = servicesTotal;
                totalbilltext.Text = (documentBill["roomCost"].AsInt32 + servicesTotal).ToString() + " đ";
            }
            else
            {
                //serviceList.Clear();
            }
        }
        public void LayDichVu(IMongoCollection<BsonDocument> collectionService)
        {
            List<BsonDocument> documentService = collectionService.Find(new BsonDocument()).ToList();
            ServiceList.Clear();
            foreach (BsonDocument service in documentService)
            {
                ServiceList.Add(new service { name = service["serviceName"].AsString, price = service["servicePrice"].AsInt32, imagesource = service["serviceImage"].AsString, quantity = 0 });
            }
        }

        public void LayPhongTrongDat()
        {
            var rFilter = Builders<BsonDocument>.Filter.Where(x => x["roomType"].AsString == roomOption.Text && x["roomState"].AsString == "Trống");
            List<BsonDocument> documentRoom = collectionRoom.Find(rFilter).ToList();
            phongTrong.Clear();
            foreach (BsonDocument room in documentRoom)
            {
                phongTrong.Add(room["roomName"].AsString);
            }
        }

        public int LayGia(string loaiPhong, string loaiThue)
        {
            List<BsonDocument> documentRoomType = collectionRoomType.Find(new BsonDocument()).ToList();
            foreach(BsonDocument room in documentRoomType) 
            {
                if(loaiPhong == room["roomType"].AsString)
                {
                    switch(loaiThue)
                    {
                        case "Theo giờ":
                            return room["firstHourPrice"].AsInt32;
                        case "Theo ngày":
                            return room["dayPrice"].AsInt32;
                        case "Qua đêm":
                            return room["nightPrice"].AsInt32;
                        default:
                            return 0;
                    }
                }
            }
            return 0;
        }

        public int LayGiaGioSau(string loaiPhong)
        {
            List<BsonDocument> documentRoomType = collectionRoomType.Find(new BsonDocument()).ToList();
            foreach (BsonDocument room in documentRoomType)
            {
                if (loaiPhong == room["roomType"].AsString)
                {
                    return room["followupHourPrice"].AsInt32;
                }
            }
            return 0;
        }

        public string LayServiceName(ObjectId serviceId, IMongoCollection<BsonDocument> collectionService)
        {
            string serviceName = "";
            List<BsonDocument> documentsService = collectionService.Find(new BsonDocument()).ToList();
            foreach (BsonDocument service in documentsService)
            {
                if (serviceId == service["_id"].AsObjectId)
                {
                    serviceName = service["serviceName"].AsString;
                    break;
                }
            }
            return serviceName;
        }

        public int LayServicePrice(ObjectId serviceId, IMongoCollection<BsonDocument> collectionService)
        {
            int servicePrice = -1;
            List<BsonDocument> documentsService = collectionService.Find(new BsonDocument()).ToList();
            foreach (BsonDocument service in documentsService)
            {
                if (serviceId == service["_id"].AsObjectId)
                {
                    servicePrice = service["servicePrice"].AsInt32;
                    break;
                }
            }
            return servicePrice;
        }

        public void UpdateHoaDon()
        {
            var queryReceipt = Builders<BsonDocument>.Filter.Eq("idCode", billBook.ID);
            var updateReceipt = Builders<BsonDocument>.Update.Set("receiptType", ServiceType.Text).Set("checkIn", DateTime.Parse(DateReceived.Text)).Set("checkOut", checkOut).Set("roomCost", Int32.Parse(RoomCost.Text)).Set("roomType", roomOption.Text).Set("receiptTotal", Int32.Parse(totalbilltext.Text)).Set("roomId", LayRoomId()).Set("receiptState", "Chưa thanh toán");
            collectionReceipt.UpdateOne(queryReceipt, updateReceipt);
        }

        public void UpdatePhong()
        {
            var rFilter = Builders<BsonDocument>.Filter.Eq("idCode", billBook.ID);
            BsonDocument receipt = collectionReceipt.Find(rFilter).FirstOrDefault();
            var queryRoom = Builders<BsonDocument>.Filter.Eq("_id", LayRoomId());
            var updateRoom = Builders<BsonDocument>.Update.Set("roomState", "Đang thuê").Set("receiptId", receipt["_id"].AsObjectId);
            collectionRoom.UpdateOne(queryRoom, updateRoom);
        }

        public ObjectId LayRoomId()
        {
            var rFilter = Builders<BsonDocument>.Filter.Eq("roomName", roomNumberOption.Text);
            BsonDocument room = collectionRoom.Find(rFilter).FirstOrDefault();
            return room["_id"].AsObjectId;
        }

        public void UpdateDichVuSuDung()
        {
            var bFilter = Builders<BsonDocument>.Filter.Eq("idCode", billBook.ID);
            BsonDocument documentReceipt = collectionReceipt.Find(bFilter).FirstOrDefault();
            if (documentReceipt["serviceId"].AsBsonArray.Count > 0)
            {
                foreach (ObjectId suId in documentReceipt["serviceId"].AsBsonArray)
                {
                    var query = Builders<BsonDocument>.Filter.Eq("_id", suId);
                    collectionServiceUsed.DeleteOne(query);
                }
                documentReceipt["serviceId"].AsBsonArray.Clear();
            }
            foreach (serviceUsed su in serviceUsedList)
            {
                if (su.soluong > 0)
                {
                    ObjectId objectId = ObjectId.GenerateNewId();
                    BsonDocument document = new BsonDocument
                        {
                            {"_id", objectId},
                            { "serviceId",LayIdDichVu(su.nameServiceUsed)},
                            { "serviceQuantity",su.soluong},
                            { "pricePerUnit",Int32.Parse(su.price)},
                        };
                    collectionServiceUsed.InsertOne(document);
                    var update = Builders<BsonDocument>.Update.Push("serviceId", objectId);
                    collectionReceipt.UpdateOne(bFilter, update);
                }
            }
        }

        public ObjectId LayIdDichVu(string tenDv)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("serviceName", tenDv);
            BsonDocument dichVu = collectionService.Find(filter).FirstOrDefault();
            return dichVu["_id"].AsObjectId;
        }



        /*
         Xử lý sự kiện
         */

        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Chitietdatphong.NavigationService.GoBack();
        }

        private void congbtnclick(object sender, RoutedEventArgs e)
        {
            int gioThue = Int32.Parse(giosudung.Text);
            if (gioThue < 8)
            {
                gioThue++;
                DateReceived.Text = System.DateTime.Now.ToString();
                DateReturned.Text = System.DateTime.Now.AddHours(gioThue).ToString();
                checkOut = System.DateTime.Now.AddHours(gioThue);
                RoomCost.Text = (LayGia(roomOption.Text, "Theo giờ") + (gioThue - 1) * LayGiaGioSau(roomOption.Text)).ToString();
                totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
                giosudung.Text = gioThue.ToString();
            }
            else
            {
                MessageBox.Show("Hình thức thuê này không được vượt 8 tiếng!", "CẢNH BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void trubtnclick(object sender, RoutedEventArgs e)
        {
            int gioThue = Int32.Parse(giosudung.Text);
            if (gioThue > 1)
            {
                gioThue--;
                DateReceived.Text = System.DateTime.Now.ToString();
                DateReturned.Text = System.DateTime.Now.AddHours(gioThue).ToString();
                checkOut = System.DateTime.Now.AddHours(gioThue);
                RoomCost.Text = (LayGia(roomOption.Text, "Theo giờ") + (gioThue - 1) * LayGiaGioSau(roomOption.Text)).ToString();
                totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
                giosudung.Text = gioThue.ToString();
            }
            else
            {
                MessageBox.Show("Hình thức thuê này không được ít hơn 1 tiếng!", "CẢNH BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void giocheck(object sender, RoutedEventArgs e)
        {
            ServiceType.Text = "Theo giờ";
            DateReceived.Text = System.DateTime.Now.ToString();
            DateReturned.Text = System.DateTime.Now.AddHours(Int32.Parse(giosudung.Text)).ToString();
            checkOut = System.DateTime.Now.AddHours(Int32.Parse(giosudung.Text));
            RoomCost.Text = (LayGia(roomOption.Text, "Theo giờ") + (Int32.Parse(giosudung.Text) - 1) * LayGiaGioSau(roomOption.Text)).ToString();
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();

            FutureDatePicker.SelectedDate = System.DateTime.Now;
            PresetTimePicker.SelectedTime = System.DateTime.Now;

            bordergio.Visibility = Visibility.Visible;
            borderngay.Visibility = Visibility.Hidden;
            borderdem.Visibility = Visibility.Hidden;
        }

        private void ngaycheck(object sender, RoutedEventArgs e)
        {
            ServiceType.Text = "Theo ngày";
            DateReceived.Text = System.DateTime.Now.ToString();
            DateReturned.Text = System.DateTime.Now.AddDays((DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days).ToString();
            checkOut = System.DateTime.Now.AddDays((DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days);
            RoomCost.Text = (LayGia(roomOption.Text, "Theo ngày") * (DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days).ToString();
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();

            borderngay.Visibility = Visibility.Visible;
            bordergio.Visibility = Visibility.Hidden;
            borderdem.Visibility = Visibility.Hidden;
        }

        private void demcheck(object sender, RoutedEventArgs e)
        {
            TimeSpan midNight = TimeSpan.Parse("00:00");
            TimeSpan now = System.DateTime.Now.TimeOfDay;
            ServiceType.Text = "Qua đêm";
            DateReceived.Text = System.DateTime.Now.ToString();
            if (now < midNight)
            {
                checkOut = System.DateTime.Now.Date.AddDays(1).AddHours(12);
            }
            else
            {
                checkOut = System.DateTime.Now.Date.AddHours(12);
            }
            DateReturned.Text = checkOut.ToString();
            RoomCost.Text = (LayGia(roomOption.Text, "Qua đêm")).ToString();
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();

            borderdem.Visibility = Visibility.Visible;
            bordergio.Visibility = Visibility.Hidden;
            borderngay.Visibility = Visibility.Hidden;
        }

        private void truservicebtn_Click(object sender, RoutedEventArgs e)
        {
            service Service = (sender as Button).DataContext as service;
            if (Service.quantity > 0)
            {
                Service.quantity--;
                foreach (serviceUsed su in serviceUsedList)
                {
                    if (su.nameServiceUsed == Service.name)
                    {
                        su.soluong = Service.quantity;
                        su.total = su.soluong * Int32.Parse(su.price);
                        break;
                    }
                }
                tongDichVu -= Service.price;
                totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
                serviceIC.Items.Refresh();
                serviceusedDG.Items.Refresh();
            }
        }

        private void congservicebtn_Click(object sender, RoutedEventArgs e)
        {
            service Service = (sender as Button).DataContext as service;
            Service.quantity++;
            bool daCoDichVu = false;
            foreach (serviceUsed su in serviceUsedList)
            {
                if (su.nameServiceUsed == Service.name)
                {
                    su.soluong = Service.quantity;
                    su.total = su.soluong * Int32.Parse(su.price);
                    daCoDichVu = true;
                    break;
                }
            }
            if (!daCoDichVu)
            {
                serviceUsedList.Add(new serviceUsed { nameServiceUsed = Service.name, price = Service.price.ToString(), soluong = Service.quantity, total = Service.quantity * Service.price });
            }
            tongDichVu += Service.price;
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
            serviceIC.Items.Refresh();
            serviceusedDG.Items.Refresh();
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
                        CustomerBirth.Text = customer["dateOfBirth"].ToLocalTime().ToShortDateString();
                        CustomerPhoneNumber.Text = customer["phoneNumber"].AsString;
                        CustomerEmail.Text = customer["email"].AsString;
                        break;
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
                        CustomerBirth.Text = customer["dateOfBirth"].ToLocalTime().ToString();
                        CustomerIdNumber.Text = customer["idNumber"].AsString;
                        CustomerEmail.Text = customer["email"].AsString;
                    }
                }
            }
        }
        
        private void nhanphong_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
            LayPhongTrongDat();
            textChonPhong.Text = "Chọn phòng: " + roomOption.Text;
            if (phongTrong.Count > 0)
            {
                roomNumberOption.Items.Refresh();
                roomNumberOption.SelectedIndex = 0;
            }
            else
            {
                roomNumberOption.Text = "Hết phòng";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;
        }

        private void chonphong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Khách thuê phòng " + roomNumberOption.Text + "?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UpdateHoaDon();
                UpdatePhong();
                UpdateDichVuSuDung();
                Dialog.IsOpen = false;
                Chitietdatphong.NavigationService.GoBack();
            }
        }

        private void roomOption_DropDownClosed(object sender, EventArgs e)
        {
            switch (roomOption.SelectedIndex)
            {
                case 0:
                    textGiaGio.Text = (LayGia(roomOption.Text, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                    textGiaNgay.Text = (LayGia(roomOption.Text, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                    textGiaDem.Text = (LayGia(roomOption.Text, "Qua đêm") / 1000.0).ToString() + "k/đêm";
                    break;
                case 1:
                    textGiaGio.Text = (LayGia(roomOption.Text, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                    textGiaNgay.Text = (LayGia(roomOption.Text, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                    textGiaDem.Text = (LayGia(roomOption.Text, "Qua đêm") / 1000.0).ToString() + "k/đêm";
                    break;
                case 2:
                    textGiaGio.Text = (LayGia(roomOption.Text, "Theo giờ") / 1000.0).ToString() + "k/giờ";
                    textGiaNgay.Text = (LayGia(roomOption.Text, "Theo ngày") / 1000.0).ToString() + "k/ngày";
                    textGiaDem.Text = (LayGia(roomOption.Text, "Qua đêm") / 1000.0).ToString() + "k/đêm";
                    break;
                default:
                    break;
            }

            DateReceived.Text = System.DateTime.Now.ToString();
            if (radiobtngio.IsChecked == true)
            {
                RoomCost.Text = (LayGia(roomOption.Text, "Theo giờ") + (Int32.Parse(giosudung.Text) - 1) * LayGiaGioSau(roomOption.Text)).ToString();
            }
            else if (radiobtnngay.IsChecked == true)
            {
                RoomCost.Text = (LayGia(roomOption.Text, "Theo ngày") * (DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days).ToString();
            }
            else
            {
                RoomCost.Text = (LayGia(roomOption.Text, "Qua đêm")).ToString();
            }
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
        }

        private void DatePicker2_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DateReceived.Text = System.DateTime.Now.ToString();
            DateReturned.Text = System.DateTime.Now.AddDays((DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days).ToString();
            checkOut = System.DateTime.Now.AddDays((DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days);
            RoomCost.Text = (LayGia(roomOption.Text, "Theo ngày") * (DatePicker2.SelectedDate.Value - DatePicker1.SelectedDate.Value).Days).ToString();
            totalbilltext.Text = (Int32.Parse(RoomCost.Text) + tongDichVu).ToString();
        }

        private void huyphongbtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Hủy hóa đơn?", "Xác nhận", button, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var bFilter = Builders<BsonDocument>.Filter.Eq("idCode", billBook.ID);
                BsonDocument documentReceipt = collectionReceipt.Find(bFilter).FirstOrDefault();
                if (documentReceipt["serviceId"].AsBsonArray.Count > 0)
                {
                    foreach (ObjectId suId in documentReceipt["serviceId"].AsBsonArray)
                    {
                        var query = Builders<BsonDocument>.Filter.Eq("_id", suId);
                        collectionServiceUsed.DeleteOne(query);
                    }
                    documentReceipt["serviceId"].AsBsonArray.Clear();
                }
                collectionReceipt.DeleteOne(bFilter);
                Chitietdatphong.NavigationService.GoBack();
            }
            
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
