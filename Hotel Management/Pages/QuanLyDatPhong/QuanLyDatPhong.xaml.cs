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

using MongoDB.Bson;
using MongoDB.Driver;

namespace Hotel_Management.Pages.QuanLyDatPhong
{
    /// <summary>
    /// Interaction logic for QuanLyDatPhong.xaml
    /// </summary>
    public partial class QuanLyDatPhong : Page
    {
        static MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
        static IMongoDatabase database = client.GetDatabase("HotelManagement");
        IMongoCollection<BsonDocument> collectionRoom = database.GetCollection<BsonDocument>("Room");
        IMongoCollection<BsonDocument> collectionReceipt = database.GetCollection<BsonDocument>("Receipt");
        IMongoCollection<BsonDocument> collectionCustomer = database.GetCollection<BsonDocument>("Customer");
        IMongoCollection<BsonDocument> collectionUser = database.GetCollection<BsonDocument>("User");
        IMongoCollection<BsonDocument> collectionServiceUsed = database.GetCollection<BsonDocument>("ServiceUsed");
        IMongoCollection<BsonDocument> collectionService = database.GetCollection<BsonDocument>("Service");
        static DateTime dateTimeChange;

        List<Bill> receiptList = new List<Bill>();

        List<Bill> receiptListDisplay = new List<Bill>() {
             new Bill() { ID="12345",   Phong="Stardard",LoaiThue="Theo giờ", Total= 100000, checkIn = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
            };
        public QuanLyDatPhong()
        {
            InitializeComponent();
            dateTimeChange = FutureDatePicker.SelectedDate.Value;
            LayHoaDonDat(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
            textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
            DGHoadon.ItemsSource = receiptListDisplay;

        }

        public void LayHoaDonDat(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, IMongoCollection<BsonDocument> collectionUser)
        {
            DateTime today = FutureDatePicker.SelectedDate.Value.Date.AddDays(0);
            DateTime tomorrow = FutureDatePicker.SelectedDate.Value.Date.AddDays(1);
            string receiptIdCode;
            string receiptType;
            int receiptTotal;
            string checkInDate = "";
            ObjectId customerId;
            string customerName;
            ObjectId userId;
            string userName;
            string roomType;
            var filterReceiptByDate = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã đặt") & Builders<BsonDocument>.Filter.Gte("checkIn", today) & Builders<BsonDocument>.Filter.Lt("checkIn", tomorrow);
            List<BsonDocument> documentsReceipt = collectionReceipt.Find(filterReceiptByDate).ToList();
            receiptList.Clear();
            receiptListDisplay.Clear();
            foreach (BsonDocument receipt in documentsReceipt)
            {
                receiptIdCode = receipt["idCode"].AsString;
                receiptType = receipt["receiptType"].AsString;
                receiptTotal = receipt["receiptTotal"].AsInt32;
                customerId = receipt["customerId"].AsObjectId;
                userId = receipt["userId"][0].AsObjectId;
                checkInDate = receipt["checkIn"].ToLocalTime().ToLongTimeString();
                userName = LayUserName(userId, collectionUser);
                customerName = LayCustomerName(customerId, collectionCustomer);
                roomType = receipt["roomType"].AsString;
                receiptListDisplay.Add(new Bill { ID = receiptIdCode, Phong = roomType, LoaiThue = receiptType, Total = receiptTotal, checkIn = checkInDate, nameCustomer = customerName, nameStaff = userName });
                receiptList.Add(new Bill { ID = receiptIdCode, Phong = roomType, LoaiThue = receiptType, Total = receiptTotal, checkIn = checkInDate, nameCustomer = customerName, nameStaff = userName });
            }
            DGHoadon.Items.Refresh();
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
        private void FutureDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
           if (dateTimeChange != FutureDatePicker.SelectedDate.Value)
            {
                LayHoaDonDat(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                DGHoadon.Items.Refresh();
                dateTimeChange = FutureDatePicker.SelectedDate.Value;
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
            textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
            DGHoadon.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Xóa hóa đơn?", "Cảnh báo", button, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Bill item = ((FrameworkElement)sender).DataContext as Bill;

                //Xóa ngoài UI (Không xóa DB)
                receiptList.Remove(item);
                receiptListDisplay.Remove(item);
                DGHoadon.ItemsSource = receiptListDisplay;



                //Xóa DB và update UI
                //collectionReceipt.DeleteOne(x => x["idCode"] == item.ID);
                //LayHoaDonDat(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);

                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                DGHoadon.Items.Refresh();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Bill item = ((FrameworkElement)sender).DataContext as Bill;
            Quanlydatphong.NavigationService.Navigate(new ChiTietDatPhong("Nhận phòng", item));
        }

        private void themdatphong_Click(object sender, RoutedEventArgs e)
        {
            Quanlydatphong.NavigationService.Navigate(new ChiTietDatPhong("Đặt phòng"));
        }
    }
    public class Bill
    {
        public string ID { get; set; }
        public string Phong { get; set; }
        public string LoaiThue { get; set; }
        public int Total { get; set; }
        public string nameCustomer { get; set; }
        public string nameStaff { get; set; }
        public string checkIn { get; set; }
    }

}
