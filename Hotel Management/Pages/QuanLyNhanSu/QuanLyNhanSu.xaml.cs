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

namespace Hotel_Management.Pages.QuanLyNhanSu
{
    /// <summary>
    /// Interaction logic for QuanLyNhanSu.xaml
    /// </summary>
    public partial class QuanLyNhanSu : Page
    {
        public QuanLyNhanSu()
        {
            InitializeComponent();
            List<bill> users = new List<bill>();
            users.Add(new bill() { ID = 1, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 2, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 3, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { ID = 4, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 5, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 6, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { ID = 7, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 8, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 9, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            users.Add(new bill() { ID = 10, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 11, Status = "Đã thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "1/1/2013" });
            users.Add(new bill() { ID = 12, Status = "Chưa thanh toán", Sum = 500000, CreateDate = "12/12/2012", PayDate = "null" });
            GD_Hoa_Don.ItemsSource = users;
        }

        public class bill
        {
            public int ID { get; set; }

            public string Status { get; set; }

            public int Sum { get; set; }

            public string CreateDate { get; set; }

            public string PayDate { get; set; }
        }
    }
}
