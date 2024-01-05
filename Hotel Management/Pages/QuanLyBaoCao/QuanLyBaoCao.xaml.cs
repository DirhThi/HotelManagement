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
using System.Windows.Forms.DataVisualization;
using LiveCharts;
using LiveCharts.Wpf;
using static Hotel_Management.Pages.QuanLyHoaDon.QuanLyHoaDon;
using static Hotel_Management.Pages.QuanLyKhachHang.QuanLyKhachHang;
using MongoDB.Driver;
using MongoDB.Bson;
using Hotel_Management.MongoDatabase;

namespace Hotel_Management.Pages.QuanLyBaoCao
{
    /// <summary>
    /// Interaction logic for QuanLyBaoCao.xaml
    /// </summary>
    public partial class QuanLyBaoCao : Page
    {
        static MongoHandler handler = MongoHandler.GetInstance();
        IMongoCollection<BsonDocument> collectionCustomer = handler.GetCollection("Customer");
        IMongoCollection<BsonDocument> collectionRoom = handler.GetCollection("Room");
        IMongoCollection<BsonDocument> collectionReceipt = handler.GetCollection("Receipt");
        IMongoCollection<BsonDocument> collectionUser = handler.GetCollection("User");
        IMongoCollection<BsonDocument> collectionServiceUsed = handler.GetCollection("ServiceUsed");
        IMongoCollection<BsonDocument> collectionService = handler.GetCollection("Service");
        IMongoCollection<BsonDocument> collectionRoomType = handler.GetCollection("RoomType");

        List<serviceReport> serviceReportList = new List<serviceReport>() {
             new serviceReport() { TenDV="Sting",   SL=10,Total=1000000},
             new serviceReport() { TenDV="Stáing",   SL=9,Total=12000000},
             new serviceReport() { TenDV="Stiang",   SL=8,Total=1000000},
             new serviceReport() { TenDV="Staing",   SL=7,Total=10000400},
             new serviceReport() { TenDV="Stding",   SL=6,Total=10020000},
             new serviceReport() { TenDV="Stasing",   SL=5,Total=1000000},
             new serviceReport() { TenDV="Stging",   SL=4,Total=10020000},
             new serviceReport() { TenDV="Stisng",   SL=4,Total=10020000},
             new serviceReport() { TenDV="Stinag",   SL=3,Total=1000000},
             new serviceReport() { TenDV="Stijng",   SL=2,Total=10004000},

            };
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now;
        SeriesCollection soLuongLoaiThue = new SeriesCollection();
        SeriesCollection doanhThuLoaiThue = new SeriesCollection();
        SeriesCollection soLuongLoaiPhong = new SeriesCollection();
        SeriesCollection doanhThuLoaiPhong = new SeriesCollection();
        List<receiptType> loaiThueList = new List<receiptType>();
        List<rooomType> loaiPhongList = new List<rooomType>();
        public QuanLyBaoCao()
        {
            InitializeComponent();
            ChartDoanhThuLoaiThue.Visibility = Visibility.Visible;
            ChartDoanhThuLoaiPhong.Visibility = Visibility.Visible;
            startDate = ngaybatdau.SelectedDate.Value.Date;
            endDate = ngayketthuc.SelectedDate.Value.Date.AddDays(1);
            ThongKeHoaDon(collectionReceipt, collectionServiceUsed);
            ThongKeDichVu(collectionReceipt, collectionServiceUsed, collectionService);
            ThongKeLoaiThue(collectionReceipt);
            ThongKeLoaiPhong(collectionReceipt, collectionRoom, collectionRoomType);
            DGdichvu.ItemsSource = serviceReportList;
        }



        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (ChartDoanhThuLoaiPhong.Visibility == Visibility.Visible)
            {
                ChartDoanhThuLoaiPhong.Visibility = Visibility.Collapsed;
                ChartSoLuongLoaiPhong.Visibility = Visibility.Visible;
            }
            else
            {
                ChartSoLuongLoaiPhong.Visibility = Visibility.Collapsed;
                ChartDoanhThuLoaiPhong.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ChartDoanhThuLoaiThue.Visibility == Visibility.Visible)
            {
                ChartDoanhThuLoaiThue.Visibility = Visibility.Collapsed;
                ChartSoLuongLoaiThue.Visibility = Visibility.Visible;
            }
            else
            {
                ChartSoLuongLoaiThue.Visibility = Visibility.Collapsed;
                ChartDoanhThuLoaiThue.Visibility = Visibility.Visible;
            }    
        }

        public void ThongKeHoaDon(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionServiceUsed)
        {
            var receiptFilter = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã thanh toán") & Builders<BsonDocument>.Filter.Gte("checkOut", startDate) & Builders<BsonDocument>.Filter.Lt("checkOut", endDate);
            List<BsonDocument> documentReceipt = collectionReceipt.Find(receiptFilter).ToList();
            textSoLuongDat.Text = documentReceipt.Count.ToString();
            int tongThuPhong = 0;
            int tongThuDichVu = 0;
            foreach(BsonDocument receipt in documentReceipt) 
            {
                tongThuPhong += receipt["roomCost"].AsInt32;
                foreach(ObjectId serviceId in receipt["serviceId"].AsBsonArray) 
                {
                    var serviceUsedFilter = Builders<BsonDocument>.Filter.Eq("_id", serviceId);
                    List<BsonDocument> documentServiceUsed = collectionServiceUsed.Find(serviceUsedFilter).ToList();
                    if(documentServiceUsed.Count > 0)
                    {
                        foreach(BsonDocument serviceUsed in documentServiceUsed)
                        {
                            tongThuDichVu += serviceUsed["pricePerUnit"].AsInt32 * serviceUsed["serviceQuantity"].AsInt32;
                        }
                    }
                }
            }
            textThuPhong.Text = tongThuPhong.ToString();
            textThuDichVu.Text = tongThuDichVu.ToString();
            textTongThu.Text = (tongThuPhong + tongThuDichVu).ToString();
        }

        public void ThongKeDichVu(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionServiceUsed, IMongoCollection<BsonDocument> collectionService)
        {
            serviceReportList.Clear();
            List<BsonDocument> documentServiceReport = collectionService.Find(new BsonDocument()).ToList();
            foreach(BsonDocument serviceReport in documentServiceReport)
            {
                serviceReportList.Add(new serviceReport { TenDV = serviceReport["serviceName"].AsString, SL = 0, Total = 0 });
            }

            var receiptFilter = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã thanh toán") & Builders<BsonDocument>.Filter.Gte("checkOut", startDate) & Builders<BsonDocument>.Filter.Lt("checkOut", endDate);
            List<BsonDocument> documentReceipt = collectionReceipt.Find(receiptFilter).ToList();
            foreach (BsonDocument receipt in documentReceipt)
            {
                foreach (ObjectId serviceId in receipt["serviceId"].AsBsonArray)
                {
                    var serviceUsedFilter = Builders<BsonDocument>.Filter.Eq("_id", serviceId);
                    List<BsonDocument> documentServiceUsed = collectionServiceUsed.Find(serviceUsedFilter).ToList();
                    if (documentServiceUsed.Count > 0)
                    {
                        foreach (BsonDocument serviceUsed in documentServiceUsed)
                        {
                            var serviceFilter = Builders<BsonDocument>.Filter.Eq("_id", serviceUsed["serviceId"]);
                            List<BsonDocument> documentService = collectionService.Find(serviceFilter).ToList();
                            foreach(BsonDocument service in  documentService)
                            {
                                foreach(serviceReport sr in serviceReportList)
                                {
                                    if(sr.TenDV == service["serviceName"].AsString)
                                    {
                                        sr.SL += serviceUsed["serviceQuantity"].AsInt32;
                                        sr.Total += serviceUsed["serviceQuantity"].AsInt32 * serviceUsed["pricePerUnit"].AsInt32;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ThongKeLoaiThue(IMongoCollection<BsonDocument> collectionReceipt)
        {
            loaiThueList.Clear();
            loaiThueList.Add(new receiptType { loaiThue = "Theo ngày", soLuong = 0, tongTien = 0 });
            loaiThueList.Add(new receiptType { loaiThue = "Theo giờ", soLuong = 0, tongTien = 0 });
            loaiThueList.Add(new receiptType { loaiThue = "Theo đêm", soLuong = 0, tongTien = 0 });

            var receiptFilter = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã thanh toán") & Builders<BsonDocument>.Filter.Gte("checkOut", startDate) & Builders<BsonDocument>.Filter.Lt("checkOut", endDate);
            List<BsonDocument> documentReceipt = collectionReceipt.Find(receiptFilter).ToList();
            foreach(BsonDocument receipt in documentReceipt) 
            {
                foreach(receiptType rt in loaiThueList)
                {
                    if(rt.loaiThue == receipt["receiptType"].AsString)
                    {
                        rt.soLuong++;
                        rt.tongTien += receipt["roomCost"].AsInt32;
                    }
                }
            }

            soLuongLoaiThue.Clear();
            doanhThuLoaiThue.Clear();

            foreach(receiptType rt in loaiThueList)
            {
                soLuongLoaiThue.Add(new PieSeries { Title = rt.loaiThue, Values = new ChartValues<int> { rt.soLuong }, DataLabels = true });
                doanhThuLoaiThue.Add(new PieSeries { Title = rt.loaiThue, Values = new ChartValues<int> { rt.tongTien }, DataLabels = true });
            }

            ChartSoLuongLoaiThue.dataChart = soLuongLoaiThue;
            ChartDoanhThuLoaiThue.dataChart = doanhThuLoaiThue;

        }

        public void ThongKeLoaiPhong(IMongoCollection<BsonDocument> collectionReceipt, IMongoCollection<BsonDocument> collectionRoom, IMongoCollection<BsonDocument> collectionSRoomType)
        {
            loaiPhongList.Clear();
            List<BsonDocument> documentRoomType = collectionRoomType.Find(new BsonDocument()).ToList();
            foreach(BsonDocument roomType in documentRoomType)
            {
                loaiPhongList.Add(new rooomType { loaiPhong = roomType["roomType"].AsString, soLuong = 0, tongTien = 0 });
            }
            var receiptFilter = Builders<BsonDocument>.Filter.Where(x => x["receiptState"] == "Đã thanh toán") & Builders<BsonDocument>.Filter.Gte("checkOut", startDate) & Builders<BsonDocument>.Filter.Lt("checkOut", endDate);
            List<BsonDocument> documentReceipt = collectionReceipt.Find(receiptFilter).ToList();
            foreach (BsonDocument receipt in documentReceipt)
            {
                var roomFilter = Builders<BsonDocument>.Filter.Eq("_id", receipt["roomId"].AsObjectId);
                List<BsonDocument> documentRoom = collectionRoom.Find(roomFilter).ToList();
                foreach(BsonDocument room in documentRoom)
                {
                    foreach(rooomType rmt in loaiPhongList)
                    {
                        if(rmt.loaiPhong == room["roomType"].AsString)
                        {
                            rmt.soLuong++;
                            rmt.tongTien += receipt["roomCost"].AsInt32;
                        }
                    }
                }
            }

            soLuongLoaiPhong.Clear();
            doanhThuLoaiPhong.Clear();

            foreach (rooomType rt in loaiPhongList)
            {
                soLuongLoaiPhong.Add(new PieSeries { Title = rt.loaiPhong, Values = new ChartValues<int> { rt.soLuong },DataLabels=true });
                doanhThuLoaiPhong.Add(new PieSeries { Title = rt.loaiPhong, Values = new ChartValues<int> { rt.tongTien }, DataLabels = true });
            }

            ChartSoLuongLoaiPhong.dataChart = soLuongLoaiPhong;
            ChartDoanhThuLoaiPhong.dataChart = doanhThuLoaiPhong;
        }

        private void ngaybatdau_CalendarClosed(object sender, RoutedEventArgs e)
        {
            startDate = ngaybatdau.SelectedDate.Value.Date;
            endDate = ngayketthuc.SelectedDate.Value.Date.AddDays(1);
            ThongKeHoaDon(collectionReceipt, collectionServiceUsed);
            ThongKeDichVu(collectionReceipt, collectionServiceUsed, collectionService);
            ThongKeLoaiThue(collectionReceipt);
            ThongKeLoaiPhong(collectionReceipt, collectionRoom, collectionRoomType);
            DGdichvu.Items.Refresh();
        }

        private void ngayketthuc_CalendarClosed(object sender, RoutedEventArgs e)
        {
            startDate = ngaybatdau.SelectedDate.Value.Date;
            endDate = ngayketthuc.SelectedDate.Value.Date.AddDays(1);
            ThongKeHoaDon(collectionReceipt, collectionServiceUsed);
            ThongKeDichVu(collectionReceipt, collectionServiceUsed, collectionService);
            ThongKeLoaiThue(collectionReceipt);
            ThongKeLoaiPhong(collectionReceipt, collectionRoom, collectionRoomType);
            DGdichvu.Items.Refresh();
        }
    }

    public class serviceReport
    {
        public string TenDV { get; set; }
        public int SL { get; set; }
        public int Total { get; set; }
    }

    public class receiptType
    {
        public string loaiThue { get; set; }
        public int soLuong {  get; set; }
        public int tongTien { get; set; }
    }

    public class rooomType
    {
        public string loaiPhong { get; set; }
        public int soLuong { get; set; }
        public int tongTien { get; set; }
    }
}
