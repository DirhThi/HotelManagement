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
        List<Bill> receiptList = new List<Bill>() /*{
             new Bill() { ID=123456,   Phong="101",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=234561,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=334551,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=412358,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=512479,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=684212,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=711144,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=851251,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=994573,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=108235,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=113463,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=199564,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=242322, Phong="102",LoaiThue="Theo giờ",Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=379655, Phong="102",LoaiThue="Theo giờ",Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=464647,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=535389,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=607554,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=737831,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=881238,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=935125,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},

            }*/;
        static DateTime dateTimeChange;
        static int tongTienNgay = 0;
        public QuanLyHoaDon()
        {
            InitializeComponent();
            dateTimeChange = FutureDatePicker.SelectedDate.Value;
            LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
            DGHoadon.ItemsSource = receiptList;
            textSoLuong.Text = "Số lượng: " + receiptList.Count.ToString();
            textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
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
            foreach(BsonDocument receipt in documentsReceipt)
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

        private void FutureDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if(dateTimeChange != FutureDatePicker.SelectedDate.Value)
            {
                tongTienNgay = 0;
                LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
                DGHoadon.ItemsSource = receiptList;
                textSoLuong.Text = "Số lượng: " + receiptList.Count.ToString();
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
                    foreach (Bill item in items)
                    {
                        receiptList.Remove(item);
                    }
                    DGHoadon.ItemsSource = receiptList;
                    textSoLuong.Text = "Số lượng: " + DGHoadon.Items.Count.ToString();
                    tongTienNgay = 0;
                    for(int i = 0; i < receiptList.Count; i++)
                    {
                        tongTienNgay += receiptList[i].Total;
                    }
                    textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
                    DGHoadon.Items.Refresh();
                }
            }
        }
    }
}
