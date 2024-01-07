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
using Hotel_Management.MongoDatabase;
using static Hotel_Management.Pages.QuanLyKhachHang.QuanLyKhachHang;

namespace Hotel_Management.Pages.QuanLyNhanSu
{
    /// <summary>
    /// Interaction logic for QuanLyNhanSu.xaml
    /// </summary>
    public partial class QuanLyNhanSu : Page
    {
        bool isEditing = false;
        static MongoHandler handler = MongoHandler.GetInstance();
        static MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
        static IMongoDatabase database = client.GetDatabase("HotelManagement");
        IMongoCollection<BsonDocument> collectionEmployee = database.GetCollection<BsonDocument>("User");
        IMongoCollection<BsonDocument> collectionCustomer = handler.GetCollection("Customer");
        IMongoCollection<BsonDocument> collectionRoom = handler.GetCollection("Room");
        IMongoCollection<BsonDocument> collectionReceipt = handler.GetCollection("Receipt");
        IMongoCollection<BsonDocument> collectionServiceUsed = handler.GetCollection("ServiceUsed");
        IMongoCollection<BsonDocument> collectionService = handler.GetCollection("Service");
        List<NhanVien> employeeList = new List<NhanVien>();/* {
             new Khachhang() { TenNV="Nguyễn Đình Thi",   Sodienthoai="0909090909",CCCD="12345678900966123", Ngaysinh= "23/12/2023", Email = "Mail@gmail.com", Role = "Nhân Viên"},
            };*/
        List<NhanVien> employeeListDisplay = new List<NhanVien>();
        NhanVien nv = new NhanVien();
        List<UsedService> serviceList = new List<UsedService>();

        List<Bill> receiptList = new List<Bill>() {
             new Bill() { ID="123456",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID="123341",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
                new Bill() { ID="123456",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID="123341",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
   new Bill() { ID="123456",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID="123341",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
   new Bill() { ID="123456",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID="123341",   Phong="101",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},

            };
        int receiptsTotal = 0;
        public QuanLyNhanSu()
        {
            InitializeComponent();
            LayNhanVien(collectionEmployee);
            textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
            DGNhanVien.ItemsSource = employeeListDisplay;
            autoorder();
        }

        private void autoorder()
        {
            int t = 1;
            for (int i = 0; i < employeeListDisplay.Count; i++)
            {
                employeeListDisplay[i].stt = t;
                t++;
            }
        }

        public void LayNhanVien(IMongoCollection<BsonDocument> collectionEmployee)
        {
            string employeeName;
            string employeePhone;
            string employeeId;
            string employeeDoB;
            string employeeEmail;
            string employeeRole;
            List<BsonDocument> documentsEmployee = collectionEmployee.Find(new BsonDocument()).ToList();
            employeeList.Clear();
            employeeListDisplay.Clear();

            foreach (BsonDocument employee in documentsEmployee)
            {
                employeeName = employee["userName"].AsString;
                employeeId = employee["idNumber"].AsString;
                employeePhone = employee["phoneNumber"].AsString;
                employeeDoB = employee["dateOfBirth"].ToLocalTime().ToShortDateString();
                employeeEmail = employee["email"].AsString;
                employeeRole = employee["userRole"].AsString;
                employeeList.Add(new NhanVien() { TenNV = employeeName, Sodienthoai = employeePhone, CCCD = employeeId, Ngaysinh = employeeDoB, Email = employeeEmail, Role = employeeRole });
                employeeListDisplay.Add(new NhanVien() { TenNV = employeeName, Sodienthoai = employeePhone, CCCD = employeeId, Ngaysinh = employeeDoB, Email = employeeEmail, Role = employeeRole });

            }

        }

        public class NhanVien
        {
            public int stt { get; set; }
            public string TenNV { get; set; }
            public string Sodienthoai { get; set; }
            public string CCCD { get; set; }
            public string Ngaysinh { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }


        }

        public class Khachhang
        {
            public int stt { get; set; }
            public string TenKH { get; set; }
            public string Sodienthoai { get; set; }
            public string CCCD { get; set; }
            public string Ngaysinh { get; set; }
            public string Email { get; set; }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DGNhanVien.SelectedItems.Count != 0)
            {
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show("Xóa nhân viên đã chọn ?", "Cảnh báo", button, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    List<NhanVien> items = DGNhanVien.SelectedItems.Cast<NhanVien>().ToList();

                    //comment đến.....
                    foreach (NhanVien item in items)
                    {
                        employeeList.Remove(item);
                        employeeListDisplay.Remove(item);

                    }
                    DGNhanVien.ItemsSource = employeeListDisplay;
                    //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db


                    /* Xóa dữ liệu trong db
                    XoaNhanVien(collectionEmployee, items);
                    LayNhanVien(collectionEmployee);
                    */

                    textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
                    DGNhanVien.ItemsSource = employeeListDisplay;
                    autoorder();
                    DGNhanVien.Items.Refresh();
                }
            }
        }

        public void XoaNhanVien(IMongoCollection<BsonDocument> collectionEmployee, List<NhanVien> items)
        {
            foreach (NhanVien item in items)
            {
                collectionEmployee.DeleteOne(x => x["idNumber"] == item.CCCD);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("Xóa nhân viên ?", "Cảnh báo", button, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                NhanVien item = ((FrameworkElement)sender).DataContext as NhanVien;

                //comment đến.....
                employeeList.Remove(item);
                employeeListDisplay.Remove(item);
                DGNhanVien.ItemsSource = employeeListDisplay;
                //...đây và bỏ comment phía dưới nếu muốn xóa dữ liệu trong db

                /* Xóa dữ liệu trong db
                collectionEmployee.DeleteOne(x => x["idNumber"] == item.CCCD);
                LayNhanVien(collectionEmployee);
                */

                textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count.ToString();
                DGNhanVien.ItemsSource = employeeListDisplay;
                autoorder();
                DGNhanVien.Items.Refresh();

            }
        }

        private void searchbox_textchanged(object sender, TextChangedEventArgs e)
        {
            employeeListDisplay.Clear();
            int count = employeeList.Count();
            string text = searchbox.Text;
            foreach (NhanVien P in employeeList)
            {
                if (P.TenNV.Contains(text))
                {
                    employeeListDisplay.Add(P);
                }
            }
            textSoLuong.Text = "Số lượng: " + employeeListDisplay.Count();
            autoorder();
            DGNhanVien.Items.Refresh();
        }

        private void EditStaff_Click(object sender, RoutedEventArgs e)
        {
            //Editing staff, set isEditing to true (then reset to false after done editing)
            isEditing = true;

            NhanVien item = ((FrameworkElement)sender).DataContext as NhanVien;
            nv = item;
            LayChiTietNhanVien(collectionEmployee, item);
            LayHoaDonNhanVien(collectionEmployee, collectionReceipt, item);
            textSoLuongHoaDon.Text = "Số lượng: " + receiptList.Count.ToString();
            textTongTienHoaDon.Text = "Tổng tiền: " + receiptsTotal.ToString();
            CustomerName.Focusable = false;
            CustomerIdNumber.Focusable = false;
            DGHoadonnhanvien.Items.Refresh();
            borderhoadon.Visibility =Visibility.Collapsed ;
            bordersuanhanvien.Visibility = Visibility.Visible;

            Dialog.IsOpen = true;

        }
        private void AddStaff_Click(object sender, RoutedEventArgs e)
        {
            borderthemnhanvien.Visibility = Visibility.Visible;
            Add_StaffName.Text = "";
            Add_StaffDoB.Text = "";
            Add_StaffPhoneNumber.Text = "";
            Add_StaffIdNumber.Text = "";
            Add_StaffEmail.Text = "";
            Add_StaffPosition.Text = "";
            Add_StaffUsername.Text = "";
            Add_StaffPass.Clear();
            isEditing = false;
            Dialog.IsOpen = true;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            bordersuanhanvien.Visibility = Visibility.Collapsed;
            borderthemnhanvien.Visibility = Visibility.Collapsed;
            borderhoadon.Visibility = Visibility.Collapsed;
            //Reset isEditing to false
            isEditing = false;
            Dialog.IsOpen = false;
        }

        public void LayChiTietNhanVien(IMongoCollection<BsonDocument> collectionEmployee, NhanVien item)
        {
            var eFilter = Builders<BsonDocument>.Filter.Eq("idNumber", item.CCCD);
            List<BsonDocument> documentsEmployee = collectionEmployee.Find(eFilter).ToList();
            foreach (BsonDocument user in documentsEmployee)
            {
                CustomerName.Text = user["userName"].AsString;
                CustomerIdNumber.Text = user["idNumber"].AsString;
                CustomerPhoneNumber.Text = user["phoneNumber"].AsString;
                CustomerEmail.Text = user["email"].AsString;
                CustomerBirth.Text = user["dateOfBirth"].ToLocalTime().ToShortDateString();
                switch (user["userRole"].AsString)
                {
                    case "Quản lý":
                        CustomerRole.SelectedIndex = 0;
                        break;
                    case "Nhân viên":
                        CustomerRole.SelectedIndex = 1;
                        break;
                    default:
                        CustomerRole.SelectedIndex = 1;
                        break;
                }
            }
        }

        public void LayHoaDonNhanVien(IMongoCollection<BsonDocument> collectionEmployee, IMongoCollection<BsonDocument> collectionReceipt, NhanVien item)
        {
            var eFilter = Builders<BsonDocument>.Filter.Eq("idNumber", item.CCCD);
            List<BsonDocument> documentsEmployee = collectionEmployee.Find(eFilter).ToList();
            foreach (BsonDocument user in documentsEmployee)
            {
                var rFilter = Builders<BsonDocument>.Filter.Where(x => x["userId"][0].AsObjectId == user["_id"].AsObjectId && x["receiptState"].AsString == "Đã thanh toán");
                List<BsonDocument> documentsReceipt = collectionReceipt.Find(rFilter).ToList();
                receiptsTotal = 0;
                if (documentsReceipt.Count > 0)
                {
                    string receiptIdCode;
                    string roomName;
                    string receiptType;
                    int receiptTotal;
                    string createDate;
                    string userName;
                    receiptList.Clear();
                    foreach (BsonDocument receipt in documentsReceipt)
                    {
                        receiptIdCode = receipt["idCode"].AsString;
                        receiptType = receipt["receiptType"].AsString;
                        receiptTotal = receipt["receiptTotal"].AsInt32;
                        createDate = receipt["createDate"].ToLocalTime().ToShortDateString();
                        roomName = LayRoomName(receipt["roomId"].AsObjectId, collectionRoom);
                        userName = LayUserName(receipt["userId"][0].AsObjectId, collectionEmployee);
                        receiptsTotal += receiptTotal;
                        receiptList.Add(new Bill { ID = receiptIdCode, CreateDate = createDate, LoaiThue = receiptType, nameCustomer = CustomerName.Text, nameStaff = userName, Phong = roomName, Total = receiptTotal });
                    }
                    DGHoadonnhanvien.ItemsSource = receiptList;
                }
                else
                {
                    receiptList.Clear();
                    DGHoadonnhanvien.Items.Refresh();
                }
            }
        }

        public string LayRoomName(ObjectId roomId, IMongoCollection<BsonDocument> collectionRoom)
        {
            string roomName = "";
            List<BsonDocument> documentsRoom = collectionRoom.Find(new BsonDocument()).ToList();
            foreach (BsonDocument room in documentsRoom)
            {
                if (roomId == room["_id"].AsObjectId)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CustomerPhoneNumber.Text) || string.IsNullOrEmpty(CustomerEmail.Text) || string.IsNullOrEmpty(CustomerBirth.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin nhân sự!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (int.TryParse(CustomerPhoneNumber.Text, out int phoneNumber) && IsValidEmail(CustomerEmail.Text) && DateTime.TryParse(CustomerBirth.Text, out DateTime date))
                {
                    //Cập nhật UI
                    foreach (NhanVien KH in employeeList)
                    {
                        if (KH.CCCD == nv.CCCD)
                        {
                            KH.Sodienthoai = CustomerPhoneNumber.Text;
                            KH.Email = CustomerEmail.Text;
                            KH.Ngaysinh = CustomerBirth.Text;
                            KH.Role = CustomerRole.Text;
                            break;
                        }
                    }
                    foreach (NhanVien KH in employeeListDisplay)
                    {
                        if (KH.CCCD == nv.CCCD)
                        {
                            KH.Sodienthoai = CustomerPhoneNumber.Text;
                            KH.Email = CustomerEmail.Text;
                            KH.Ngaysinh = CustomerBirth.Text;
                            KH.Role = CustomerRole.Text;
                            break;
                        }
                    }


                    //Cập nhật DB + UI
                    /*
                    var query = Builders<BsonDocument>.Filter.Eq("idNumber", nv.CCCD);
                    var update = Builders<BsonDocument>.Update.Set("phoneNumber", CustomerPhoneNumber).Set("email", CustomerEmail.Text).Set("dateOfBirth", date).Set("userRole", CustomerRole.Text);
                    collectionUser.UpdateOne(query, update);
                    LayNhanVien(collectionEmployee);
                    */

                    DGNhanVien.Items.Refresh();
                    Dialog.IsOpen = false;

                }
                else
                {
                    MessageBox.Show("Thông tin nhập không hợp lệ! Vui lòng thử lại", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (int.TryParse(CustomerPhoneNumber.Text, out phoneNumber) == false)
                    {
                        CustomerPhoneNumber.Clear();
                    }
                    if (IsValidEmail(CustomerEmail.Text) == false)
                    {
                        CustomerEmail.Clear();
                    }
                    if (DateTime.TryParse(CustomerBirth.Text, out date) == false)
                    {
                        CustomerBirth.Clear();
                    }

                }
            }
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
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

        private void DGHoadonnhanvien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bordersuanhanvien.Visibility = Visibility.Collapsed;
            borderhoadon.Visibility = Visibility.Visible;
            Bill item = DGHoadonnhanvien.SelectedItem as Bill;
            if (item != null)
            {
                LayChiTietHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionEmployee, collectionService, collectionServiceUsed, item);
            }
        }

        
        public void LayChiTietHoaDon(IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionCustomer, IMongoCollection<BsonDocument> collectionUser, IMongoCollection<BsonDocument> collectionService, IMongoCollection<BsonDocument> collectionServiceUsed, Bill item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("idCode", item.ID);
            List<BsonDocument> documentsReceipt = collectionReceipt.Find(filter).ToList();
            foreach (BsonDocument receipt in documentsReceipt)
            {
                CreatedDate.Text = receipt["createDate"].ToLocalTime().ToString();
                Receiptionist.Text = LayUserName(receipt["userId"][0].AsObjectId, collectionUser);
                ServiceType.Text = receipt["receiptType"].AsString;
                DateReceived.Text = receipt["checkIn"].ToLocalTime().ToString();
                DateReturned.Text = receipt["checkOut"].ToLocalTime().ToString();
                RoomCost.Text = receipt["roomCost"].ToString() + " đ";
                RoomName.Text = LayRoomName(receipt["roomId"].AsObjectId, collectionRoom) + " - " + LayRoomType(receipt["roomId"].AsObjectId, collectionRoom);
                CustomerNameOnReceipt.Text = LayCustomerName(receipt["customerId"].AsObjectId, collectionCustomer);
                if (receipt["serviceId"].AsBsonArray.Count > 0)
                {
                    serviceList.Clear();
                    int serviceQuantity = -1;
                    string serviceName = "";
                    int servicePrice = -1;
                    int serviceTotal = -1;
                    int servicesTotal = 0;
                    foreach (ObjectId sId in receipt["serviceId"].AsBsonArray)
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
                        serviceList.Add(new UsedService { nameServiceUsed = serviceName, price = servicePrice.ToString(), soluong = serviceQuantity.ToString(), total = serviceTotal.ToString() + " đ" });
                    }
                    serviceusedDG.ItemsSource = serviceList;
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
        

        private void AcceptAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                //Logic for editing
            }
            else
            {
                //Logic for adding
                if (
                Add_StaffName.Text == "" ||
                Add_StaffDoB.Text == "" ||
                Add_StaffPhoneNumber.Text == "" ||
                Add_StaffIdNumber.Text == "" ||
                Add_StaffEmail.Text == "" ||
                Add_StaffPosition.Text == "" ||
                Add_StaffUsername.Text == ""
                )
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                }
                else
                {
                    if (handler != null)
                    {
                        IMongoCollection<BsonDocument> collection = handler.GetCollection("User");
                        List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                        foreach (BsonDocument user in documents)
                        {

                            if (user["idNumber"].AsString == Add_StaffIdNumber.Text)
                            {
                                MessageBox.Show("Nhân sự này đã có trong danh sách.");
                            }
                        }
                        var newDoc = new BsonDocument
                        {
                            {"userName", Add_StaffName.Text },
                            { "dateOfBirth", Convert.ToDateTime(Add_StaffDoB.Text)},
                            {"phoneNumber", Add_StaffPhoneNumber.Text },
                            {"idNumber", Add_StaffIdNumber.Text },
                            {"email", Add_StaffEmail.Text },
                            {"receiptId", new BsonArray() },
                            { "userRole", Add_StaffPosition.Text},
                            {"accountId", Add_StaffUsername.Text },
                            {"accountPassword", Add_StaffPass.Password }

                        };
                        collection.InsertOne(newDoc);
                        LayNhanVien(handler.GetCollection("User"));
                        DGNhanVien.Items.Refresh();
                        Dialog.IsOpen = false;
                    }
                }


            }
        }

        private void CancelHoadon(object sender, RoutedEventArgs e)
        {
            borderhoadon.Visibility = Visibility.Collapsed;
            bordersuanhanvien.Visibility = Visibility.Visible;
        }

        private void DGNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGNhanVien.SelectedItems.Count >= 1)
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
