using Hotel_Management.MongoDatabase;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;


namespace Hotel_Management.Pages.QuanLyThuePhong
{
    /// <summary>
    /// Interaction logic for QuanLyThuePhong.xaml
    /// </summary>
    public partial class QuanLyThuePhong : Page
    {
        List<int> phongItems = new List<int>();
        List<Phong> phongtrongList = new List<Phong>() /*{
            new Phong() { maphong = "101", loaiphong = "Standard",tenkhachhang="Nguyễn Đình Thi",sodienthoai="0909090909",ListCsvc=new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" } },
         ư
        }*/;

        List<Phong> phongthueList = new List<Phong>() /*{
}*/;

        List<Phong> phongdatList = new List<Phong>() /*{
          }*/;

        List<Phong> phongbaotriList = new List<Phong>() /*{
  
        }*/;
        public QuanLyThuePhong()
        {
            InitializeComponent();
            phongtrongIC.ItemsSource = phongtrongList;
            phongthueIC.ItemsSource = phongthueList;
            phongdatIC.ItemsSource = phongdatList;
            phongbaotriIC.ItemsSource = phongbaotriList;
            MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("HotelManagement");
            IMongoCollection<BsonDocument> collectionRoom = database.GetCollection<BsonDocument>("Room");
            IMongoCollection<BsonDocument> collectionRoomType = database.GetCollection<BsonDocument>("RoomType");
            IMongoCollection<BsonDocument> collectionFurniture = database.GetCollection<BsonDocument>("Furniture");
            IMongoCollection<BsonDocument> collectionReceipt = database.GetCollection<BsonDocument>("Receipt");
            IMongoCollection<BsonDocument> collectionCustomer = database.GetCollection<BsonDocument>("Customer");
            LayPhongTrong(collectionRoom, collectionRoomType, collectionFurniture);
            LayPhongThue(collectionRoom, collectionRoomType, collectionFurniture, collectionReceipt, collectionCustomer);
            LayPhongDat(collectionRoom, collectionRoomType, collectionFurniture, collectionReceipt, collectionCustomer);
            LayPhongBaoTri(collectionRoom, collectionRoomType, collectionFurniture);
            //lấy dữ liệu các phòng thêm vào list tương ứng đã tạo bên trên .
 
        }

        private void Phong_click(object sender, RoutedEventArgs e)
        {
            Phong PhongDuocChon = (sender as Button).DataContext as Phong;
            // Lấy dữ liệu của phòng thôn qua item.maphong sau đó gán mã phòng,trạng thái nếu phòng đã đặt hoặc dã thuê phải có thông tin thuê, thiếu gì tự thêm vào
            string maphong = PhongDuocChon.maphong;
            string loaiphong = PhongDuocChon.loaiphong;
            string trangthai = "phongthue";
            string loaithue = "";

            quanlythuephong.NavigationService.Navigate(new ChiTietPhong(maphong, loaiphong, trangthai, loaithue));
        }

        private void Phongbaotri_click(object sender, RoutedEventArgs e)
        {
            ConfirmStateButton.Tag = ((Phong)((sender as Button).DataContext)).maphong;

             var filter = Builders<BsonDocument>.Filter.Eq("roomName", ((Phong)((sender as Button).DataContext)).maphong);
            BsonDocument room= MongoHandler.GetInstance().GetCollection("Room").Find(filter).FirstOrDefault();
            if(room!=null)
            {
                if (room["roomState"] == "Đang dọn dẹp")
                {
                    radiobtndondep.IsChecked = true;
                }    
                else if (room["roomState"] == "Đang bảo trì")
                {
                    radiobtnbaotri.IsChecked = true;
                }    
            }    

            Dialog.IsOpen = true;
        }

        private void sortphongtrong_click(object sender, RoutedEventArgs e)
        {
            if (sortphongtrongtb.Text == "Mã phòng")
            {
                sortphongtrongtb.Text = "Loại phòng";
                phongtrongList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongtrongIC.Items.Refresh();
            }
            else
            {
                sortphongtrongtb.Text = "Mã phòng";
                phongtrongList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongtrongIC.Items.Refresh();
            }
        }

        private void sortphongthue_click(object sender, RoutedEventArgs e)
        {
            if (sortphongthuetb.Text == "Mã phòng")
            {
                sortphongthuetb.Text = "Loại phòng";
                phongthueList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongthueIC.Items.Refresh();
            }
            else
            {
                sortphongthuetb.Text = "Mã phòng";
                phongthueList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongthueIC.Items.Refresh();
            }
        }

        private void sortphongdat_click(object sender, RoutedEventArgs e)
        {
            if (sortphongdattb.Text == "Mã phòng")
            {
                sortphongdattb.Text = "Loại phòng";
                phongdatList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongdatIC.Items.Refresh();
            }
            else
            {
                sortphongdattb.Text = "Mã phòng";
                phongdatList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongdatIC.Items.Refresh();
            }
        }

        private void sortphongbaotri_click(object sender, RoutedEventArgs e)
        {
            if (sortphongbaotritb.Text == "Mã phòng")
            {
                sortphongbaotritb.Text = "Loại phòng";
                phongbaotriList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongbaotriIC.Items.Refresh();
            }
            else
            {
                sortphongbaotritb.Text = "Mã phòng";
                phongbaotriList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongbaotriIC.Items.Refresh();
            }
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;
        }

        public void LayDanhSachCSVC(List<string> roomFurniture, string roomType, IMongoCollection<BsonDocument> collectionRoomType, List<BsonDocument> documentsFurniture)
        {
            var filterRoomType = Builders<BsonDocument>.Filter.Eq("roomType", roomType);
            var type = collectionRoomType.Find(filterRoomType).FirstOrDefault();
            roomFurniture.Clear();
            foreach (ObjectId furnitureId in type["furnituresId"].AsBsonArray)
            {
                foreach (BsonDocument furniture in documentsFurniture)
                {
                    if (furnitureId == furniture["_id"].AsObjectId)
                    {
                        roomFurniture.Add(furniture["furnitureName"].AsString);
                    }
                }
            }
        }

        public void LayPhongTrong(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionRoomType, IMongoCollection<BsonDocument> collectionFurniture)
        {
            string roomNumber;
            string roomType;
            var filterEmptyRoom = Builders<BsonDocument>.Filter.Eq("roomState", "Trống");
            List<BsonDocument> documentsEmptyRoom = collectionRoom.Find(filterEmptyRoom).ToList();
            List<BsonDocument> documentsFurniture = collectionFurniture.Find(new BsonDocument()).ToList();
            phongtrongList.Clear();
            foreach (BsonDocument room in documentsEmptyRoom)
            {
                roomNumber = room["roomName"].AsString;
                roomType = room["roomType"].AsString;
                List<string> roomFurniture = new List<string>();
                LayDanhSachCSVC(roomFurniture, roomType, collectionRoomType, documentsFurniture);
                phongtrongList.Add(new Phong { maphong = roomNumber, loaiphong = roomType, ListCsvc = roomFurniture });
            }

        }

        public void LayPhongThue(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionRoomType, IMongoCollection<BsonDocument> collectionFurniture, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer)
        {
                ObjectId roomId;
                string roomNumber;
                string roomType;
                ObjectId customerId;
                string customerName = "";
                string customerPhone = "";
                var filterEmptyRoom = Builders<BsonDocument>.Filter.Eq("roomState", "Đang thuê");
                List<BsonDocument> documentsEmptyRoom = collectionRoom.Find(filterEmptyRoom).ToList();
                List<BsonDocument> documentsFurniture = collectionFurniture.Find(new BsonDocument()).ToList();
                List<BsonDocument> documentsReceipt = collectionReceipt.Find(new BsonDocument()).ToList();
                List<BsonDocument> documentsCustomer = collectionCustomer.Find(new BsonDocument()).ToList();
                phongthueList.Clear();
                foreach (BsonDocument room in documentsEmptyRoom)
                {
                    roomId = room["_id"].AsObjectId;
                    roomNumber = room["roomName"].AsString;
                    roomType = room["roomType"].AsString;
                    List<string> roomFurniture = new List<string>();
                    LayDanhSachCSVC(roomFurniture, roomType, collectionRoomType, documentsFurniture);
                    foreach (BsonDocument receipt in documentsReceipt)
                    {
                        if (roomId == receipt["roomId"].AsObjectId && receipt["receiptState"].AsString == "Chưa thanh toán")
                        {
                            customerId = receipt["customerId"].AsObjectId;
                            foreach (BsonDocument customer in documentsCustomer)
                            {
                                if (customerId == customer["_id"].AsObjectId)
                                {
                                    customerName = customer["customerName"].AsString;
                                    customerPhone = customer["phoneNumber"].AsString;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    phongthueList.Add(new Phong { maphong = roomNumber, loaiphong = roomType, ListCsvc = roomFurniture, tenkhachhang = customerName, sodienthoai = customerPhone });
                }
            phongthueIC.Items.Refresh();

        }

        public async void LayPhongDat(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionRoomType, IMongoCollection<BsonDocument> collectionFurniture, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer)
        {
                ObjectId roomId;
                string roomNumber;
                string roomType;
                ObjectId customerId;
                string customerName = "";
                string customerPhone = "";
                var filterEmptyRoom = Builders<BsonDocument>.Filter.Eq("roomState", "Đã đặt");
                List<BsonDocument> documentsEmptyRoom = collectionRoom.Find(filterEmptyRoom).ToList();
                List<BsonDocument> documentsFurniture = collectionFurniture.Find(new BsonDocument()).ToList();
                List<BsonDocument> documentsReceipt = collectionReceipt.Find(new BsonDocument()).ToList();
                List<BsonDocument> documentsCustomer = collectionCustomer.Find(new BsonDocument()).ToList();
                phongdatList.Clear();
                foreach (BsonDocument room in documentsEmptyRoom)
                {
                    roomId = room["_id"].AsObjectId;
                    roomNumber = room["roomName"].AsString;
                    roomType = room["roomType"].AsString;
                    List<string> roomFurniture = new List<string>();
                    LayDanhSachCSVC(roomFurniture, roomType, collectionRoomType, documentsFurniture);
                    foreach (BsonDocument receipt in documentsReceipt)
                    {
                        if (roomId == receipt["roomId"].AsObjectId && receipt["receiptState"].AsString == "Chưa thanh toán")
                        {
                            customerId = receipt["customerId"].AsObjectId;
                            foreach (BsonDocument customer in documentsCustomer)
                            {
                                if (customerId == customer["_id"].AsObjectId)
                                {
                                    customerName = customer["customerName"].AsString;
                                    customerPhone = customer["phoneNumber"].AsString;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    phongdatList.Add(new Phong { maphong = roomNumber, loaiphong = roomType, ListCsvc = roomFurniture, tenkhachhang = customerName, sodienthoai = customerPhone });
                }
            
            phongdatIC.Items.Refresh();
        }

        public async void LayPhongBaoTri(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionRoomType, IMongoCollection<BsonDocument> collectionFurniture)
        {
                string roomNumber;
                string roomType;
                string roomState;
                var filterEmptyRoom = Builders<BsonDocument>.Filter.Where(x => x["roomState"] == "Đang bảo trì" || x["roomState"] == "Đang dọn dẹp");
                List<BsonDocument> documentsEmptyRoom = collectionRoom.Find(filterEmptyRoom).ToList();
                List<BsonDocument> documentsFurniture = collectionFurniture.Find(new BsonDocument()).ToList();
                phongbaotriList.Clear();
                foreach (BsonDocument room in documentsEmptyRoom)
                {
                    roomNumber = room["roomName"].AsString;
                    roomType = room["roomType"].AsString;
                    roomState = room["roomState"].AsString;
                    List<string> roomFurniture = new List<string>();
                    LayDanhSachCSVC(roomFurniture, roomType, collectionRoomType, documentsFurniture);
                    phongbaotriList.Add(new Phong { maphong = roomNumber, loaiphong = roomType, ListCsvc = roomFurniture, tenkhachhang = roomState });
                }
            phongbaotriIC.Items.Refresh();
        }

        private void XacNhanSuaTrangThai_Click(object sender, RoutedEventArgs e)
        {
            string maphong = (string)(sender as Button).Tag;

            IMongoCollection<BsonDocument> roomCollection = MongoHandler.GetInstance().GetCollection("Room");
            var filter = Builders<BsonDocument>.Filter.Eq(r => r["roomName"], maphong);
            string currentStatus = (roomCollection.Find(filter).FirstOrDefault())["roomState"].AsString;
            string newStatus;
            if (radiobtnsansang.IsChecked == true)
            {
                newStatus = "Trống";
            }
            else if (radiobtndondep.IsChecked == true)
            {
                newStatus = "Đang dọn dẹp";
            }
            else
            {
                newStatus = "Đang bảo trì";
            }

            if (currentStatus != newStatus)
            {
                var update = Builders<BsonDocument>.Update.Set(r => r["roomState"], newStatus);
                roomCollection.UpdateOne(filter, update);
                IMongoCollection<BsonDocument> collectionRoom = MongoHandler.GetInstance().GetCollection("Room");
                IMongoCollection<BsonDocument> collectionRoomType = MongoHandler.GetInstance().GetCollection("RoomType");
                IMongoCollection<BsonDocument> collectionFurniture = MongoHandler.GetInstance().GetCollection("Furniture");
                LayPhongTrong(collectionRoom, collectionRoomType, collectionFurniture);
                LayPhongBaoTri(collectionRoom, collectionRoomType, collectionFurniture);
                Dialog.IsOpen = false;
            }

        }

        private void tabcontrolcontry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            phongtrongIC.Items.Refresh();
            phongthueIC.Items.Refresh();
            phongdatIC.Items.Refresh() ;
            phongbaotriIC.Items.Refresh();
        }
    }

    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string tenkhachhang { get; set; }
        public string sodienthoai { get; set; }
        public List<string> ListCsvc { get; set; }

    }


}
