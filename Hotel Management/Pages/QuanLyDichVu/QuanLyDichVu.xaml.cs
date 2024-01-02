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

namespace Hotel_Management.Pages.QuanLyDichVu
{
    /// <summary>
    /// Interaction logic for QuanLyDichVu.xaml
    /// </summary>
    public partial class QuanLyDichVu : Page
    {
        List<service> ServiceList = new List<service> {
              new service {name="Nước suối",price=10000,quantity=0,imagesource="/Assets/Images/aquafina.png"},
              new service {name="Sting",price=15000,quantity=0,imagesource="/Assets/Images/sting.jpg"},
              new service {name="Cocacola",price=15000,quantity=0,imagesource="/Assets/Images/cocacola.png"},
              new service {name="Redbull",price=20000,quantity=0,imagesource="/Assets/Images/redbull.png"},
              new service {name="Mì ly",price=15000,quantity=0,imagesource="/Assets/Images/mily.png"},
              new service {name="Bún bò",price=40000,quantity=0,imagesource="/Assets/Images/bunbo.png"},
              new service {name="Phở bò",price=40000,quantity=0,imagesource="/Assets/Images/phobo.png"},

            };
        public QuanLyDichVu()
        {
            InitializeComponent();
            serviceIC.ItemsSource = ServiceList;

        }

        private void themDV_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;

        }

        private void editService_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
        }
    }

    public class service
    {
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string imagesource { get; set; }
    }
}
