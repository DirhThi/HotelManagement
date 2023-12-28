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
using System.Windows.Shapes;

namespace Hotel_Management.Pages.QuanLyCacPhong
{
    /// <summary>
    /// Interaction logic for SuaPhong.xaml
    /// </summary>
    public partial class SuaPhong : Window
    {
        private int roomId;
        public SuaPhong(int id)
        {
            InitializeComponent();
            this.roomId = id;
            Load();
        }
        
        void Load()
        {
            // lấy data room từ room id
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
