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
    /// Interaction logic for ChartDoanhThuLoaiThue.xaml
    /// </summary>
    public partial class ChartDoanhThuLoaiThue : UserControl
    {
        public ChartDoanhThuLoaiThue()
        {
            InitializeComponent();
            bieudodoanhthuloaithue();

        }


        public void bieudodoanhthuloaithue()
        {

            dataChart = new SeriesCollection
           {
               new PieSeries
               {
                   Title="Theo ngày",
                   Values= new ChartValues<int> {3200000},
                   DataLabels=true,
               },
               new PieSeries
               {
                   Title="Theo giờ",
                   Values= new ChartValues<int> {2800000},
                   DataLabels=true,
               },
                new PieSeries
               {
                   Title="Qua đêm",
                   Values= new ChartValues<int> {4000000},
                   DataLabels=true,
               },
           };
            DataContext = this;
        }



        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection dataChart { get; set; }


    }
}
