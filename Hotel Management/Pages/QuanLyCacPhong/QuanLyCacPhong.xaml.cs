using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_Management.Pages.QuanLyCacPhong
{
    /// <summary>
    /// Interaction logic for QuanLyCacPhong.xaml
    /// </summary>
    public partial class QuanLyCacPhong : Page
    {
        public QuanLyCacPhong()
        {
            InitializeComponent();
            LoadRoom();

        }
        
        void LoadRoom()
        {
            SPListRoom.Children.Clear();
            List<Room> listRoom = GetListRoom();
            foreach (int floor in listRoom.Select(room => room.Floor).Distinct())
            {
                Grid floorGrid = new Grid() { Margin=new Thickness(10)};

                floorGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                floorGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                StackPanel floorStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                TextBlock textBlock = new TextBlock() { Text = $"Tầng {floor}", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5, 0, 5, 0), FontSize = 20, FontWeight = FontWeights.SemiBold };         
                Button newRoom = new Button() { Width = 20, Height = 20, Margin = new Thickness(5, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
                newRoom.Click += NewRoomInFloorClick;

                floorStackPanel.Children.Add(textBlock);
                floorStackPanel.Children.Add(newRoom);

                StackPanel roomStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                foreach (Room room in listRoom.Where(room => room.Floor == floor))
                {
                    Button btnRoom = new Button() { Content = room.Name,Tag=room.Id };
                    btnRoom.Click += EditRoomClick;
                    roomStackPanel.Children.Add(btnRoom);
                }

                Grid.SetRow(floorStackPanel, 0);
                Grid.SetRow(roomStackPanel, 1);

                floorGrid.Children.Add(floorStackPanel);
                floorGrid.Children.Add(roomStackPanel);

                SPListRoom.Children.Add(floorGrid);
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

        private void EditRoomClick(object sender, RoutedEventArgs e)
        {                    
            SuaPhong f = new SuaPhong((int)(sender as Button).Tag);
            if ((bool)f.ShowDialog())
            {
                LoadRoom();
            }

        }
        private void NewRoomInFloorClick(object sender, RoutedEventArgs e)
        {
            ThemPhong f = new ThemPhong();
            if ((bool)f.ShowDialog())
            {
                LoadRoom();
            }

        }
        private void AddNewRoomTypeClick(object sender, RoutedEventArgs e)
        {
            ThemLoaiPhong f = new ThemLoaiPhong();
            if ((bool)f.ShowDialog())
            {
            }
        }

        private void EditRoomTypeClick(object sender, RoutedEventArgs e)
        {
            SuaLoaiPhong f = new SuaLoaiPhong();
            if ((bool)f.ShowDialog())
            {
            }
        }

        private void AddNewFloorClick(object sender, RoutedEventArgs e)
        {
            ThemTang f = new ThemTang();
            if ((bool)f.ShowDialog())
            {
            }
        }
    }
}
