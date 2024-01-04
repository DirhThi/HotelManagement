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

namespace Hotel_Management.Pages.QuanLyBaoCao
{
    /// <summary>
    /// Interaction logic for QuanLyBaoCao.xaml
    /// </summary>
    public partial class QuanLyBaoCao : Page
    {
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
        public QuanLyBaoCao()
        {
            InitializeComponent();
            ChartDoanhThuLoaiThue.Visibility = Visibility.Visible;
            ChartDoanhThuLoaiPhong.Visibility = Visibility.Visible;
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


    }

    public class serviceReport
    {
        public string TenDV { get; set; }
        public int SL { get; set; }
        public int Total { get; set; }
    }
}
