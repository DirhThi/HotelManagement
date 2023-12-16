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

namespace Hotel_Management.Pages.QuanLyBaoCao
{
    /// <summary>
    /// Interaction logic for QuanLyBaoCao.xaml
    /// </summary>
    public partial class QuanLyBaoCao : Page
    {
        List<ChartValue> chartValues;
        List<ChartValue> pieValue;
        public QuanLyBaoCao()
        {
            InitializeComponent();
            ColumnChart.ChartAreas[0].AxisX.Title = "Ngày";
            ColumnChart.ChartAreas[0].AxisY.Title = "Doanh thu (nghìn đồng)";

            chartValues = new List<ChartValue>();
            chartValues.Add(new ChartValue("1", 300));
            chartValues.Add(new ChartValue("2", 500));
            chartValues.Add(new ChartValue("3", 360));
            chartValues.Add(new ChartValue("4", 440));
            chartValues.Add(new ChartValue("5", 100));
            chartValues.Add(new ChartValue("6", 900));
            chartValues.Add(new ChartValue("7", 500));
            chartValues.Add(new ChartValue("8", 260));
            chartValues.Add(new ChartValue("9", 340));
            chartValues.Add(new ChartValue("10", 330));
            chartValues.Add(new ChartValue("11", 520));

            pieValue = new List<ChartValue>();
            pieValue.Add(new ChartValue("Phòng 101", 0.2));
            pieValue.Add(new ChartValue("Phòng 102", 0.2));
            pieValue.Add(new ChartValue("Phòng 103", 0.1));
            pieValue.Add(new ChartValue("Phòng 104", 0.3));
            pieValue.Add(new ChartValue("Phòng 105", 0.15));
            pieValue.Add(new ChartValue("Phòng 106", 0.2));
            pieValue.Add(new ChartValue("Phòng 107", 0.3));
            pieValue.Add(new ChartValue("Phòng 108", 0.4));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColumnChart.Titles.Count == 0)
            {
                ColumnChart.Titles.Add("Doanh thu phòng");
            }
            else
            {
                ColumnChart.Titles[0].Text = "Doanh thu phòng";
            }

            if (PieChart.Titles.Count == 0)
            {
                PieChart.Titles.Add("Doanh thu phòng");
            }
            else
            {
                PieChart.Titles[0].Text = "Doanh thu phòng";
            }
            ColumnChart.Series[0].Points.Clear();
            for(int i=0; i < chartValues.Count; i++)
            {
                ColumnChart.Series[0].Points.Add(chartValues[i].Value).AxisLabel = chartValues[i].ValueLable;
            }
            PieChart.Series[0].Points.Clear();
            for (int i = 0; i < pieValue.Count; i++)
            {
                PieChart.Series[0].Points.Add(pieValue[i].Value).AxisLabel = pieValue[i].ValueLable;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ColumnChart.Titles.Count == 0)
            {
                ColumnChart.Titles.Add("Doanh thu dịch vụ");
            }
            else
            {
                ColumnChart.Titles[0].Text = "Doanh thu dịch vụ";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ColumnChart.Titles.Count == 0)
            {
                ColumnChart.Titles.Add("Tổng doanh thu");
            }
            else
            {
                ColumnChart.Titles[0].Text = "Tổng doanh thu";
            }
        }
    }

    public class ChartValue
    {
        public string ValueLable { get; set; }
        public double Value { get; set; }

        public ChartValue(string label, double value) 
        {
            ValueLable = label;
            Value = value;
        }
    }
}
