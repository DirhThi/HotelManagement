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
using LiveCharts;
using LiveCharts.Wpf;

namespace Hotel_Management.Pages.QuanLyBaoCao
{
    /// <summary>
    /// Interaction logic for ChartSoLuongLoaiPhong.xaml
    /// </summary>
    public partial class ChartSoLuongLoaiPhong : UserControl
    {
        public ChartSoLuongLoaiPhong()
        {
            InitializeComponent();
            datachartconfig();

        }


        public void datachartconfig()
        {

            dataChart = new SeriesCollection
           {
               new PieSeries
               {
                   Title="Standard",
                   Values= new ChartValues<int> {8},
                   DataLabels=true,
               },
               new PieSeries
               {
                   Title="Vip",
                   Values= new ChartValues<int> {6},
                   DataLabels=true,
               },
                new PieSeries
               {
                   Title="Deluxe",
                   Values= new ChartValues<int> {10},
                   DataLabels=true,
               },
           };
            DataContext = this;
        }



        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection dataChart { get; set; }

    }
}
