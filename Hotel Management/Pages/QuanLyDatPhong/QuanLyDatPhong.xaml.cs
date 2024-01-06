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

namespace Hotel_Management.Pages.QuanLyDatPhong
{
    /// <summary>
    /// Interaction logic for QuanLyDatPhong.xaml
    /// </summary>
    public partial class QuanLyDatPhong : Page
    {
        List<Bill> receiptList = new List<Bill>();

        List<Bill> receiptListDisplay = new List<Bill>() {
             new Bill() { ID="12345",   Phong="Stardard",LoaiThue="Theo giờ", Total= 100000, CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
            };
        public QuanLyDatPhong()
        {
            InitializeComponent();
            DGHoadon.ItemsSource = receiptListDisplay;

        }
        private void FutureDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
           /* if (dateTimeChange != FutureDatePicker.SelectedDate.Value)
            {
                tongTienNgay = 0;
                LayHoaDon(collectionRoom, collectionReceipt, collectionCustomer, collectionUser);
                DGHoadon.ItemsSource = receiptListDisplay;
                textSoLuong.Text = "Số lượng: " + receiptListDisplay.Count.ToString();
                textTongTien.Text = "Tổng tiền: " + tongTienNgay.ToString();
                DGHoadon.Items.Refresh();
                dateTimeChange = FutureDatePicker.SelectedDate.Value;
            }*/
        }
        private void searchbox_textchanged(object sender, TextChangedEventArgs e)
        {
           /* receiptListDisplay.Clear();
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
           */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {/*
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
*/
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Quanlydatphong.NavigationService.Navigate(new ChiTietDatPhong("Nhận phòng"));
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
        public string CreateDate { get; set; }
    }

}
