﻿using System;
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
            }*/;
        List<Bill> receiptListDisplay = new List<Bill>();
        static DateTime dateTimeChange;
        static int tongTienNgay = 0;
        public QuanLyHoaDon()
        {
            InitializeComponent();
            dateTimeChange = FutureDatePicker.SelectedDate.Value;
            LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
            DGHoadon.ItemsSource = receiptListDisplay;
            textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
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
            receiptListDisplay.Clear();
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
                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                textTongTien.Text = "Tổng tiền: " + receiptListDisplay.ToString();
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
                    DGHoadon.ItemsSource = receiptListDisplay;
                    textSoLuong.Text = "Số lượng: " + DGHoadon.Items.Count.ToString();
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
            DGHoadon.Items.Refresh();
        }
    }
}
