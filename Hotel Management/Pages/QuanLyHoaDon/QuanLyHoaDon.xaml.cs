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
using static Hotel_Management.Pages.QuanLyKhachHang.QuanLyKhachHang;
using Hotel_Management.Pages.QuanLyCacPhong;

namespace Hotel_Management.Pages.QuanLyHoaDon
{
    /// <summary>
    /// Interaction logic for QuanLyHoaDon.xaml
    /// </summary>
    public partial class QuanLyHoaDon : Page
    {
        static MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
        static IMongoDatabase database = client.GetDatabase("HotelManagement");
        IMongoCollection<BsonDocument> collectionRoom = database.GetCollection<BsonDocument>("Room");
        IMongoCollection<BsonDocument> collectionReceipt = database.GetCollection<BsonDocument>("Receipt");
        IMongoCollection<BsonDocument> collectionCustomer = database.GetCollection<BsonDocument>("Customer");
        IMongoCollection<BsonDocument> collectionUser = database.GetCollection<BsonDocument>("User");
        IMongoCollection<BsonDocument> collectionServiceUsed = database.GetCollection<BsonDocument>("ServiceUsed");
        IMongoCollection<BsonDocument> collectionService = database.GetCollection<BsonDocument>("Service");
        List<Bill> receiptList = new List<Bill>() /*{
             new Bill() { ID=123456,   Phong="101",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
            }*/;
        List<Bill> receiptListDisplay = new List<Bill>();
        List<UsedService> serviceList = new List<UsedService>();
        static DateTime dateTimeChange;
        static int tongTienNgay = 0;
        public QuanLyHoaDon()
        {
            InitializeComponent();
            SetRole();
            dateTimeChange = FutureDatePicker.SelectedDate.Value;
            LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
            DGHoadon.ItemsSource = receiptListDisplay;
            textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
            textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
        }

        private void SetRole()
        {
            string currentRole = Auth.Login.currentUser.UserRole;
            if (currentRole == "Nhân viên")
            {
             //   deletebtnDG.Visibility = Visibility.Collapsed;
                DGHoadon.SelectionMode = DataGridSelectionMode.Single;
                DGHoadon.Columns[7].Visibility = Visibility.Collapsed;
                DGHoadon.Columns[8].Visibility = Visibility.Visible;
                txtHDXoa.Visibility= Visibility.Collapsed;

            }

        }

        public void LayHoaDon(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, IMongoCollection<BsonDocument> collectionUser)
        {
            DateTime today = FutureDatePicker.SelectedDate.Value.Date.AddDays(0);
            DateTime tomorrow = FutureDatePicker.SelectedDate.Value.Date.AddDays(1);
            string receiptIdCode;
            string roomName;
            ObjectId roomId;
            string receiptType;
            int receiptTotal;
            string createDate = "";
            ObjectId customerId;
            string customerName;
            ObjectId userId;
            string userName;
            var filterReceiptByDate = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã thanh toán") & Builders<BsonDocument>.Filter.Gte("checkOut", today) & Builders<BsonDocument>.Filter.Lt("checkOut", tomorrow);
            List<BsonDocument> documentsReceipt = collectionReceipt.Find(filterReceiptByDate).ToList();
            receiptList.Clear();
            receiptListDisplay.Clear();
            tongTienNgay = 0;
            foreach (BsonDocument receipt in documentsReceipt)
            {
                receiptIdCode = receipt["idCode"].AsString;
                roomId = receipt["roomId"].AsObjectId;
                receiptType = receipt["receiptType"].AsString;
                receiptTotal = receipt["receiptTotal"].AsInt32;
                tongTienNgay += receiptTotal;
                customerId = receipt["customerId"].AsObjectId;
                userId = receipt["userId"][0].AsObjectId;
                createDate = receipt["createDate"].ToLocalTime().ToShortDateString();
                roomName = LayRoomName(roomId, collectionRoom);
                userName = LayUserName(userId, collectionUser);
                DateTime temp = receipt["createDate"].ToLocalTime();
                customerName = LayCustomerName(customerId, collectionCustomer);

                receiptListDisplay.Add(new Bill { ID = receiptIdCode, Phong = roomName, LoaiThue = receiptType, Total = receiptTotal, CreateDate = createDate, nameCustomer = customerName, nameStaff = userName });
                receiptList.Add(new Bill{ ID=receiptIdCode, Phong = roomName, LoaiThue = receiptType, Total = receiptTotal, CreateDate = createDate, nameCustomer = customerName, nameStaff = userName});
            }
            
        }

        public string LayRoomName(ObjectId roomId, IMongoCollection<BsonDocument> collectionRoom)
        {
            string roomName = "";
            List<BsonDocument> documentsRoom = collectionRoom.Find(new BsonDocument()).ToList();
            foreach(BsonDocument room in documentsRoom)
            {
                if(roomId == room["_id"].AsObjectId)
                {
                    roomName = room["roomName"].AsString;
                    break;
                }
            }
            return roomName;
        }

        public string LayRoomType(ObjectId roomId, IMongoCollection<BsonDocument> collectionRoom)
        {
            string roomType = "";
            List<BsonDocument> documentsRoom = collectionRoom.Find(new BsonDocument()).ToList();
            foreach (BsonDocument room in documentsRoom)
            {
                if (roomId == room["_id"].AsObjectId)
                {
                    roomType = room["roomType"].AsString;
                    break;
                }
            }
            return roomType;
        }

        public string LayUserName(ObjectId userId, IMongoCollection<BsonDocument> collectionUser)
        {
            string userName = "";
            List<BsonDocument> documentsUser = collectionUser.Find(new BsonDocument()).ToList();
            foreach (BsonDocument user in documentsUser)
            {
                if (userId == user["_id"].AsObjectId)
                {
                    userName = user["userName"].AsString;
                    break;
                }
            }
            return userName;
        }

        public string LayCustomerName(ObjectId customerId, IMongoCollection<BsonDocument> collectionCustomer)
        {
            string customerName = "";
            List<BsonDocument> documentsCustomer = collectionCustomer.Find(new BsonDocument()).ToList();
            foreach (BsonDocument customer in documentsCustomer)
            {
                if (customerId == customer["_id"].AsObjectId)
                {
                    customerName = customer["customerName"].AsString;
                    break;
                }
            }
            return customerName;
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

        public class Bill
        {
            public string ID { get; set; }
            public string Phong { get; set; }
            public string LoaiThue { get; set; }
            public int Total { get; set; }
            public string nameCustomer { get; set; }
            public string nameStaff { get; set; }
            public string CreateDate { get; set; }
        }

        public class UsedService
        {
            public string nameServiceUsed { get; set; }

            public string price { get; set; }

            public string soluong { get; set; }

            public string total { get; set; }
        }

        private void FutureDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if(dateTimeChange != FutureDatePicker.SelectedDate.Value)
            {
                tongTienNgay = 0;
                LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
                DGHoadon.Items.Refresh();
                dateTimeChange = FutureDatePicker.SelectedDate.Value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if(DGHoadon.SelectedItems.Count != 0)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result =  MessageBox.Show("Xóa hóa đơn đã chọn ?", "Cảnh báo", button, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                    List<Bill> items = DGHoadon.SelectedItems.Cast<Bill>().ToList();
                    
                    //comment đến.....
                    foreach (Bill item in items)
                    {
                        receiptList.Remove(item);
                        receiptListDisplay.Remove(item);

                    }
                    tongTienNgay = 0;
                    for(int i = 0; i < receiptListDisplay.Count; i++)
                    {
                        tongTienNgay += receiptListDisplay[i].Total;
                    }
                    //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db


                    /* Xóa dữ liệu trong db
                    XoaHoaDon(collectionReceipt, items);
                    LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
                    */


                    DGHoadon.ItemsSource = receiptListDisplay;
                    textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                    textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
                    DGHoadon.Items.Refresh();

                }
            }
        }

        public void XoaHoaDon(IMongoCollection<BsonDocument> collectionReceipt, List<Bill> items)
        {
            foreach(Bill item in items)
            {
                collectionReceipt.DeleteOne(x => x["idCode"] == item.ID);
            }
        }

        private void searchbox_textchanged(object sender, TextChangedEventArgs e)
        {
            receiptListDisplay.Clear();
            int count = receiptList.Count();
            string text = searchbox.Text;
            foreach (Bill P in receiptList)
            {
                if (P.ID.Contains(text))
                {
                    receiptListDisplay.Add(P);
                }
            }
            tongTienNgay = 0;
            for (int i = 0; i < receiptListDisplay.Count; i++)
            {
                tongTienNgay += receiptListDisplay[i].Total;
            }
            textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
            textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
            DGHoadon.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //receiptListDisplay.RemoveAt(0);
            //DGHoadon.Items.Refresh();

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Xóa hóa đơn?", "Cảnh báo", button, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Bill item = ((FrameworkElement)sender).DataContext as Bill;
                
                //Xóa ngoài UI (Không xóa DB)
                receiptList.Remove(item);
                receiptListDisplay.Remove(item);
                DGHoadon.ItemsSource = receiptListDisplay;
                tongTienNgay = 0;
                for (int i = 0; i < receiptListDisplay.Count; i++)
                {
                    tongTienNgay += receiptListDisplay[i].Total;
                }
                

                //Xóa DB và update UI
                //collectionReceipt.DeleteOne(x => x["idCode"] == item.ID);
                //LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);

                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
                DGHoadon.Items.Refresh();

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Bill item = ((FrameworkElement)sender).DataContext as Bill;
            LayChiTietHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser, collectionService, collectionServiceUsed, item);
            Dialog.IsOpen = true;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {

            Dialog.IsOpen = false;
        }

        public void LayChiTietHoaDon(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, IMongoCollection<BsonDocument> collectionUser, IMongoCollection<BsonDocument> collectionService, IMongoCollection<BsonDocument> collectionServiceUsed, Bill item)
        {
            /*
            CustomerNameOnReceipt
            */
            var filter = Builders<BsonDocument>.Filter.Eq("idCode", item.ID);
            List<BsonDocument> documentsReceipt = collectionReceipt.Find(filter).ToList();
            foreach(BsonDocument receipt in documentsReceipt) 
            {
                CreatedDate.Text = receipt["createDate"].ToLocalTime().ToString();
                Receiptionist.Text = LayUserName(receipt["userId"][0].AsObjectId, collectionUser);
                ServiceType.Text = receipt["receiptType"].AsString;
                DateReceived.Text = receipt["checkIn"].ToLocalTime().ToString();
                DateReturned.Text = receipt["checkOut"].ToLocalTime().ToString();
                RoomCost.Text = receipt["roomCost"].ToString() + " đ";
                RoomName.Text = LayRoomName(receipt["roomId"].AsObjectId, collectionRoom) + " - " + LayRoomType(receipt["roomId"].AsObjectId, collectionRoom);
                CustomerNameOnReceipt.Text = LayCustomerName(receipt["customerId"].AsObjectId,collectionCustomer);
                if (receipt["serviceId"].AsBsonArray.Count > 0)
                {
                    serviceList.Clear();
                    int serviceQuantity = -1;
                    string serviceName = "";
                    int servicePrice = -1;
                    int serviceTotal = -1;
                    int servicesTotal = 0;
                    foreach(ObjectId sId in receipt["serviceId"].AsBsonArray)
                    {
                        var sFilter = Builders<BsonDocument>.Filter.Where(a => a["_id"].AsObjectId == sId);
                        List<BsonDocument> documentsServiceUsed = collectionServiceUsed.Find(sFilter).ToList();
                        foreach(BsonDocument service in documentsServiceUsed)
                        {
                            serviceQuantity = service["serviceQuantity"].AsInt32;
                            serviceName = LayServiceName(service["serviceId"].AsObjectId, collectionService);
                            servicePrice = LayServicePrice(service["serviceId"].AsObjectId, collectionService);
                            serviceTotal = serviceQuantity * servicePrice;
                            servicesTotal += serviceTotal;
                        }
                        serviceList.Add(new UsedService { nameServiceUsed = serviceName, price = servicePrice.ToString(), soluong = serviceQuantity.ToString(), total = serviceTotal.ToString() + " đ" });
                    }
                    serviceusedDG.ItemsSource= serviceList;
                    serviceusedDG.Items.Refresh();
                    totalbilltext.Text = (receipt["roomCost"].AsInt32 + servicesTotal).ToString() + " đ";
                }
                else
                {
                    serviceList.Clear();
                    totalbilltext.Text = RoomCost.Text;
                }
            }
        }

        private void DGHoadon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentRole = Auth.Login.currentUser.UserRole;
            if (DGHoadon.SelectedItems.Count >= 1 && currentRole != "Nhân viên")
            {
                deletebtn.Visibility = Visibility.Visible;
            }
            else
            {
                deletebtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
