using Hotel_Management.MongoDatabase;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hotel_Management.Pages.QuanLyCacPhong
{
    /// <summary>
    /// Interaction logic for QuanLyCacPhong.xaml
    /// </summary>
    public partial class QuanLyCacPhong : Page
    {
        List<string> QLloaiPhongList = new List<string> { "Thêm phòng mới" };

        List<string> trangthaiPhongList = new List<string> { "Trống", "Đang dọn dẹp", "Đang bảo trì" };

        List<string> loaiPhongList = new List<string>();
        List<string> ListCSVC = new List<string>();
        List<Phong> phongList = new List<Phong>();
        List<Phong> phongListDisplay = new List<Phong>();

        List<string> ListCSVChientai = new List<string>();

        /*
        List<string> loaiPhongList = new List<string> { "Standard", "Deluxe", "Vip" };      
        List<string> ListCSVC = new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi", "Tủ lạnh", "Máy lạnh", "Wifi" };
        List<string> ListCSVChientai = new List<string> { "Bàn", "Ghế", "Tivi", "Giường đôi" };
              
        List<Phong> phongList = new List<Phong> {
           new Phong() { maphong = "101", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "102", loaiphong = "Standard",trangthai = "Đang bảo trì"},
            new Phong() { maphong = "104", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "201", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "203", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "204", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "301", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "302", loaiphong = "Standard",trangthai = "Phòng trống"},
            new Phong() { maphong = "401", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "402", loaiphong = "Deluxe",trangthai = "Phòng trống"},
            new Phong() { maphong = "502", loaiphong = "Vip",trangthai = "Phòng trống"},
            new Phong() { maphong = "503", loaiphong = "Vip",trangthai = "Phòng trống"},
            new Phong() { maphong = "504", loaiphong = "Vip",trangthai = "Phòng trống"},
        };
        */

        public QuanLyCacPhong()
        {
            InitializeComponent();
            GetListPhong();
            GetLoaiPhongList();
            GetListCSVC();
            phongIC.ItemsSource = phongListDisplay;
            CBLoaiPhong.ItemsSource = loaiPhongList;
            CBLoaiPhong2.ItemsSource = loaiPhongList;
            foreach (var item in loaiPhongList)
            {
                QLloaiPhongList.Add(item);
            }
            CBLoaiPhong3.ItemsSource = QLloaiPhongList;

            CBtrangthai.ItemsSource = trangthaiPhongList;
            CSVCChips.ItemsSource = ListCSVChientai;
            AllCSVCChips.ItemsSource = ListCSVC;
            suaCSVC.ItemsSource = ListCSVC;
        }



        private void Phong_click(object sender, RoutedEventArgs e)
        {
            Phong PhongDuocChon = (sender as Button).DataContext as Phong;
            string maphong = PhongDuocChon.maphong;
            string loaiphong = PhongDuocChon.loaiphong;
            string trangthai = PhongDuocChon.trangthai;
            titleDialogPhong.Text = $"Chỉnh sửa phòng {maphong}";
            CBLoaiPhong.SelectedItem = loaiphong;
            CBtrangthai.SelectedItem = trangthai;
            dialogSuaPhong.Visibility = Visibility.Visible;

            BtnSuaTrangThaiPhong.Tag = maphong;
            if (!trangthaiPhongList.Contains(trangthai))
            {
                CBLoaiPhong.IsEnabled = false;
                CBtrangthai.IsEnabled = false;
                BtnSuaTrangThaiPhong.IsEnabled = false;
            }
            else
            {
                CBLoaiPhong.IsEnabled = true;
                CBtrangthai.IsEnabled = true;
                BtnSuaTrangThaiPhong.IsEnabled = true;
            }

            Dialog.IsOpen = true;
        }
        
      

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;
            dialogSuaPhong.Visibility = Visibility.Collapsed;
            dialogThemphong.Visibility = Visibility.Collapsed;
            dialogThemTepphong.Visibility = Visibility.Collapsed;
            dialogSualoaiphong.Visibility = Visibility.Collapsed;
            dialogSuaCSVC.Visibility = Visibility.Collapsed;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortCB.SelectedIndex == 0)
            {
                phongList.Sort((left, right) => left.maphong.CompareTo(right.maphong));
                phongIC.Items.Refresh();


            }
            if (sortCB.SelectedIndex == 1)
            {
                phongList.Sort((left, right) => left.loaiphong.CompareTo(right.loaiphong));
                phongIC.Items.Refresh();
            }
            if (sortCB.SelectedIndex == 2)
            {
                phongList.Sort((left, right) => left.trangthai.CompareTo(right.trangthai));
                phongIC.Items.Refresh();
            }
        }
        List<Room> GetListRoom()
        {
            // lấy list từ database
            List<Room> list = new List<Room>();
            list.Add(new Room(1, "102", 1, "Normal", "Availble"));
            list.Add(new Room(2, "104", 1, "Normal", "Availble"));
            list.Add(new Room(3, "106", 1, "Normal", "Availble"));
            list.Add(new Room(4, "202", 2, "Normal", "Availble"));
            list.Add(new Room(5, "204", 2, "Normal", "Availble"));
            list.Add(new Room(6, "206", 2, "Normal", "Availble"));

            return list;
        }

        



        private void themphong_click(object sender, RoutedEventArgs e)
        {
            dialogThemphong.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }

        private void openfiledialog_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                tbfilename.Text = Path.GetFileName(openFileDialog.FileName);
                //List<Room> PhongMoi = new List<Room>();
                List<Phong> PhongMoi = new List<Phong>();
                foreach (string line in File.ReadLines(openFileDialog.FileName))
                {
                    string[] parts = line.Split('/');
                    //   PhongMoi.Add(new Room() {Id=System.Convert.ToInt32(parts[0]),Type=parts[1]});
                    PhongMoi.Add(new Phong() { maphong = parts[0], loaiphong = parts[1] });
                }
                phongmoiDG.ItemsSource = PhongMoi;
            }

        }

        private void themtepphong_click(object sender, RoutedEventArgs e)
        {

            dialogThemTepphong.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }

        private void QLloaiphong_click(object sender, RoutedEventArgs e)
        {
            dialogSualoaiphong.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }

        private void CSVCLoaiphong_DeleteClick(object sender, RoutedEventArgs e)
        {

            string CSVC = (sender as MaterialDesignThemes.Wpf.Chip).DataContext as string;
            ListCSVChientai.Remove(CSVC);
            CSVCChips.Items.Refresh();
        }

        private void AllChipsServiceClick(object sender, RoutedEventArgs e)
        {
            string CSVC = (sender as MaterialDesignThemes.Wpf.Chip).DataContext as string;
            if (ListCSVChientai.IndexOf(CSVC) == -1)
            {
                ListCSVChientai.Add(CSVC);
                CSVCChips.Items.Refresh();

            }
        }

        private void CSVC_DeleteClick(object sender, RoutedEventArgs e)
        {
            string CSVC = (sender as MaterialDesignThemes.Wpf.Chip).DataContext as string;
            ListCSVC.Remove(CSVC);
            suaCSVC.Items.Refresh();
        }

        private void QlyCSVC_Click(object sender, RoutedEventArgs e)
        {
            dialogSuaCSVC.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }

        private void ThemCSVC_click(object sender, RoutedEventArgs e)
        {
            if (TenCSVC_TB.Text.Length > 0)
            {
                if (ListCSVC.IndexOf(TenCSVC_TB.Text) == -1)
                {
                    ListCSVC.Add(TenCSVC_TB.Text);
                    suaCSVC.Items.Refresh();
                    TenCSVC_TB.Text = null;
                }
                else
                {
                    MessageBox.Show("Đã tồn tại CSVC");
                }
            }
        }

        private void tooltips_ME(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Popup.PlacementTarget = tooltipsThemtepphong;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
        }

        private void tooltips_ML(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        void GetListPhong()
        {
            phongList.Clear();
            List<BsonDocument> rooms = MongoHandler.GetInstance().GetCollection("Room").Find(new BsonDocument()).ToList();
            foreach (BsonDocument room in rooms)
            {
                string maphong = room["roomName"].AsString;
                string loaiphong = room["roomType"].AsString;
                string trangthai = room["roomState"].AsString;
                phongList.Add(new Phong { maphong = maphong, loaiphong = loaiphong, trangthai = trangthai });
                phongListDisplay.Add(new Phong { maphong = maphong, loaiphong = loaiphong, trangthai = trangthai });

            }
            phongIC.Items.Refresh();
        }
        void GetListCSVC()
        {
            ListCSVC.Clear();
            List<BsonDocument> CSVCs = MongoHandler.GetInstance().GetCollection("Furniture").Find(new BsonDocument()).ToList();
            foreach (BsonDocument CSVC in CSVCs)
            {
                string furnitureName = CSVC["furnitureName"].AsString;
                ListCSVC.Add(furnitureName);
            }
        }
        void GetLoaiPhongList()
        {
            loaiPhongList.Clear();
            List<BsonDocument> roomTypeList = MongoHandler.GetInstance().GetCollection("RoomType").Find(new BsonDocument()).ToList();
            foreach (BsonDocument roomType in roomTypeList)
            {
                string roomTypeName = roomType["roomType"].AsString;
                loaiPhongList.Add(roomTypeName);
            }
        }
        void GetCSVCInRoomType(string Type)
        {
            IMongoCollection<BsonDocument> roomTypeCollection = MongoHandler.GetInstance().GetCollection("RoomType");
            IMongoCollection<BsonDocument> furnitureCollection = MongoHandler.GetInstance().GetCollection("Furniture");

            var filterRoomType = Builders<BsonDocument>.Filter.Eq("roomType", Type);
            BsonDocument roomType = roomTypeCollection.Find(filterRoomType).FirstOrDefault();
            if (roomType != null)
            {
                var furnitureIds = roomType["furnituresId"].AsBsonArray.Select(id => id.AsObjectId).ToList();
                ListCSVChientai.Clear();
                foreach (var id in furnitureIds)
                {
                    var filterFurniture = Builders<BsonDocument>.Filter.Eq("_id", id);
                    BsonDocument furniture = furnitureCollection.Find(filterFurniture).FirstOrDefault();
                    if (furniture != null)
                    {
                        string furnitureName = furniture["furnitureName"].AsString;
                        ListCSVChientai.Add(furnitureName);
                    }
                }
            }
            CSVCChips.Items.Refresh();
        }

        private void SuaTrangThaiPhong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
                return;

            string maphong = (BtnSuaTrangThaiPhong.Tag).ToString();
            var filterPhong = Builders<BsonDocument>.Filter.Eq("roomName", maphong);
            BsonDocument room = MongoHandler.GetInstance().GetCollection("Room").Find(filterPhong).First();
            if (room["roomState"].ToString() != CBtrangthai.SelectedItem.ToString())
            {
                var updateState = Builders<BsonDocument>.Update.Set("roomState", CBtrangthai.SelectedItem.ToString());
                MongoHandler.GetInstance().GetCollection("Room").UpdateOne(filterPhong, updateState);
            }
            GetListPhong();
        }

        private void XacNhanThemPhong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
                return;

            if(TBMaPhong.Text==""||CBLoaiPhong2.SelectedIndex==-1)
            {
                MessageBox.Show("Lỗi mã phòng hoặc loại phòng");
                return;
            }    
            var filterIsExit = Builders<BsonDocument>.Filter.Eq("roomName", TBMaPhong.Text);
            IMongoCollection<BsonDocument> roomCollection = MongoHandler.GetInstance().GetCollection("Room");
            if (roomCollection.Find(filterIsExit).FirstOrDefault() == null)
            {
                roomCollection.InsertOne(new BsonDocument { { "roomName", TBMaPhong.Text }, { "roomType", CBLoaiPhong2.SelectedItem.ToString() }, { "roomState", "Phòng trống" } });
                GetListPhong();
            }
            else
            {
                MessageBox.Show("Mã phòng đã tồn tại!");
            }
        }

        private void CBLoaiPhong3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBLoaiPhong3.SelectedIndex == 0)
            {
                TBGiaTheoGio.Text = "";
                TBGiaTheoNgay.Text = "";
                TBGiaGioSau.Text = "";
                TBGiaQuaDem.Text = "";
            }
            else
            {
                List<BsonDocument> roomTypeList = MongoHandler.GetInstance().GetCollection("RoomType").Find(new BsonDocument()).ToList();
                foreach (BsonDocument roomType in roomTypeList)
                {
                    if (CBLoaiPhong3.SelectedItem.ToString() == roomType["roomType"].AsString)
                    {
                        TBGiaTheoGio.Text = roomType["firstHourPrice"].ToString();
                        TBGiaTheoNgay.Text = roomType["dayPrice"].ToString();
                        TBGiaGioSau.Text = roomType["followupHourPrice"].ToString();
                        TBGiaQuaDem.Text = roomType["nightPrice"].ToString();

                        GetCSVCInRoomType(roomType["roomType"].AsString);
                    }
                }
            }
        }


        private void CapNhapCSVC_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
                return;

            IMongoCollection<BsonDocument> CSVCCollection = MongoHandler.GetInstance().GetCollection("Furniture");
            List<BsonDocument> CSVCs = CSVCCollection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument CSVC in CSVCs)
            {
                string furnitureName = CSVC["furnitureName"].ToString();
                if (!ListCSVC.Contains(furnitureName))
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("furnitureName", furnitureName);
                    CSVCCollection.DeleteOne(filter);
                }
            }

            foreach (var item in ListCSVC)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("furnitureName", item);
                var result = CSVCCollection.Find(filter).FirstOrDefault();

                if (result == null)
                {
                    CSVCCollection.InsertOne(new BsonDocument { { "furnitureName", item } });
                }
            }
        }

        private void SuaLoaiPhong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
                return;

            IMongoCollection<BsonDocument> roomTypeCollection = MongoHandler.GetInstance().GetCollection("RoomType");
            IMongoCollection<BsonDocument> CSVCCollection = MongoHandler.GetInstance().GetCollection("Furniture");

            List<BsonDocument> roomTypeList = roomTypeCollection.Find(new BsonDocument()).ToList();
            List<ObjectId> ListIdCSVC = new List<ObjectId>();

            foreach (var item in ListCSVChientai)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("furnitureName", item);
                BsonDocument csvc = CSVCCollection.Find(filter).FirstOrDefault();
                if (csvc != null)
                {
                    ListIdCSVC.Add(csvc["_id"].AsObjectId);
                }
            }

            if (CBLoaiPhong3.SelectedIndex == 0)
            {
                BsonDocument newRoomType = new BsonDocument {
                    { "roomType", TBTenLoaiPhong.Text },
                    { "firstHourPrice", Convert.ToInt32(TBGiaTheoGio.Text) },
                    { "followupHourPrice", Convert.ToInt32(TBGiaGioSau.Text) },
                    { "dayPrice ", Convert.ToInt32(TBGiaTheoNgay.Text) },
                    { "nightPrice", Convert.ToInt32(TBGiaQuaDem.Text) },
                    {"furnituresId", new BsonArray(ListIdCSVC) }
                };
                roomTypeCollection.InsertOne(newRoomType);
            }
            else if (CBLoaiPhong3.SelectedIndex != -1)
            {
                foreach (BsonDocument roomType in roomTypeList)
                {
                    if (CBLoaiPhong3.SelectedItem.ToString() == roomType["roomType"].AsString)
                    {
                        var filterPhong = Builders<BsonDocument>.Filter.Eq("roomType", roomType["roomType"].AsString);
                        var updateState = Builders<BsonDocument>.Update.Set("firstHourPrice", Convert.ToInt32(TBGiaTheoGio.Text))
                                                                        .Set("followupHourPrice", Convert.ToInt32(TBGiaGioSau.Text))
                                                                        .Set("dayPrice", Convert.ToInt32(TBGiaTheoNgay.Text))
                                                                        .Set("nightPrice", Convert.ToInt32(TBGiaQuaDem.Text))
                                                                        .Set("furnituresId", new BsonArray(ListIdCSVC));
                        roomTypeCollection.UpdateOne(filterPhong, updateState);

                    }
                }
            }


        }

        private void XacNhanThemTepPhong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
                return;
            IMongoCollection<BsonDocument> roomCollection = MongoHandler.GetInstance().GetCollection("Room");
            List<Phong> listphong = (List<Phong>)phongmoiDG.ItemsSource;
            foreach(Phong phong in listphong)
            {
                roomCollection.InsertOne(new BsonDocument { {"roomName",phong.maphong },{"roomType",phong.loaiphong },{ "roomState","Trống"} });
            }
            GetListPhong();
        }


        private void Searchbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            phongListDisplay.Clear();
            int count = phongList.Count();
            string text = serchbox.Text;
            foreach(Phong P in phongList)
            {
                if(P.maphong.StartsWith(text))
                {
                    phongListDisplay.Add(P);
                }    
            }    
            phongIC.Items.Refresh();

        }
    }
    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string trangthai { get; set; }

    }
}
