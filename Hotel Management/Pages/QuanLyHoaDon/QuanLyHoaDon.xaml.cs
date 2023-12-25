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

            List<bill> users = new List<bill>();
            users.Add(new bill() { Number = 1, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013"});
            users.Add(new bill() { Number = 2, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 3, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { Number = 4, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 5, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 6, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { Number = 7, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 8, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 9, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { Number = 10, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 11, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { Number = 12, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            GD_Hoa_Don.ItemsSource = users;

            
        }

        public class bill
        {
            public int Number { get; set; }

            public string Status { get; set; }

            public int Sum { get; set; }

            public string CreateDate { get; set; }

            public string PayDate { get; set; }
        }
    }
}
