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

namespace Hotel_Management.Pages.QuanLyHoaDon
{
    /// <summary>
    /// Interaction logic for QuanLyHoaDon.xaml
    /// </summary>
    public partial class QuanLyHoaDon : Page
    {
        public QuanLyHoaDon()
        {
            InitializeComponent();

            List<Bill> users = new List<Bill> {
             new Bill() { ID=123456,   Phong="101",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=234561,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=334551,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=412358,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=512479,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=684212,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=711144,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=851251,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=994573,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=108235,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=113463,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=199564,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=242322, Phong="102",LoaiThue="Theo giờ",Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=379655, Phong="102",LoaiThue="Theo giờ",Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=464647,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=535389,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=607554,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=737831,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=881238,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},
             new Bill() { ID=935125,Phong="102",LoaiThue="Theo giờ", Total= "100000", CreateDate = "23/12/2023",nameCustomer = "Nguyễn Đình Thi",nameStaff = "Lễ tân 1"},

            };

            DGHoadon.ItemsSource = users;

            
        }

        public class Bill
        {
            public int ID { get; set; }
            public string Phong { get; set; }
            public string LoaiThue { get; set; }
            public string Total { get; set; }
            public string CreateDate { get; set; }
            public string nameCustomer { get; set; }
            public string nameStaff { get; set; }
        }
    }
}
