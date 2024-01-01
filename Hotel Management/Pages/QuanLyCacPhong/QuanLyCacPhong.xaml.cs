using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.Primitives;


namespace Hotel_Management.Pages.QuanLyCacPhong
{
    /// <summary>
    /// Interaction logic for QuanLyCacPhong.xaml
    /// </summary>
    public partial class QuanLyCacPhong : Page
    {
        List<string> QLloaiPhongList = new List<string> { "Thêm phòng mới"};
        List<string> loaiPhongList = new List<string> { "Standard", "Deluxe", "Vip" };
        List<string> trangthaiPhongList = new List<string> { "Phòng trống", "Đang dọn dẹp", "Đang bảo trì" };
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

        public QuanLyCacPhong()
        {
            InitializeComponent();
            phongIC.ItemsSource = phongList;
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
            Room PhongDuocChon = (sender as Button).DataContext as Room;
            int maphong = PhongDuocChon.Id;
            string loaiphong = PhongDuocChon.Type;
            string trangthai = PhongDuocChon.Status;
            titleDialogPhong.Text = $"Chỉnh sửa phòng {maphong}";
            CBLoaiPhong.SelectedItem = loaiphong;
            CBtrangthai.SelectedItem = trangthai;
            dialogSuaPhong.Visibility = Visibility.Visible;
            Dialog.IsOpen = true;
        }
        
      
        private void CancelDialog(object sender, RoutedEventArgs e)
        {
           
            Dialog.IsOpen = false;
            dialogSuaPhong.Visibility = Visibility.Collapsed;
            dialogThemphong.Visibility = Visibility.Collapsed;
            dialogThemTepphong.Visibility = Visibility.Collapsed;
            dialogSualoaiphong.Visibility = Visibility.Collapsed;

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
                List<Room> PhongMoi = new List<Room>();
                foreach (string line in File.ReadLines(openFileDialog.FileName))
                {
                    string[] parts = line.Split('/');
                    //PhongMoi.Add(new Room() {Id=System.Convert.ToInt32(parts[0]),Type=parts[1]});
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
            if(TenCSVC_TB.Text.Length>0)
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
    

       

    }
    public class Phong
    {
        public string maphong { get; set; }
        public string loaiphong { get; set; }
        public string trangthai { get; set; }

    }

}
