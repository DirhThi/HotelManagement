using System;
using Hotel_Management.MongoDatabase;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
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
        bool isEditing = false;
        MongoHandler handler = MongoHandler.GetInstance();
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
            UpdateData();
            serviceIC.ItemsSource = ServiceList;

        }
        private void UpdateData()
        {
            if (handler != null)
            {
                ServiceList.Clear();
                IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                foreach (BsonDocument service in documents)
                {
                    string imageSource;
                    if (service["serviceImage"].AsString == "")
                    {
                        imageSource = "/Assets/Images/addImage.png";
                    }
                    else
                    {
                        imageSource = service["serviceImage"].AsString;
                    }
                    ServiceList.Add(new service { name = service["serviceName"].AsString, price = service["servicePrice"].AsInt32, quantity = 0, imagesource = imageSource });
                }
                serviceIC.Items.Refresh();
            }
        }

        private void themDV_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
            image_serviceImage.Source = new BitmapImage(new Uri("/Assets/Images/addImage.png", UriKind.RelativeOrAbsolute));
            textBox_serviceName.Text = "";
            textBox_servicePrice.Text = "";
            textBox_serviceName.Focusable = true;
            isEditing = false;
        }

        private void CancelDialog(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = false;
            isEditing = false;
        }

        private void editService_Click(object sender, RoutedEventArgs e)
        {
            Dialog.IsOpen = true;
            isEditing = true;
            textBox_serviceName.Focusable = false;
            service currentService = (sender as Button).DataContext as service;
            textBox_serviceName.Text = currentService.name;
            textBox_servicePrice.Text = currentService.price.ToString();
            image_serviceImage.Source = new BitmapImage(new Uri(currentService.imagesource, UriKind.RelativeOrAbsolute));
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                if (String.IsNullOrEmpty(textBox_serviceName.Text) || String.IsNullOrEmpty(textBox_servicePrice.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                }
                else
                {
                    if (handler != null)
                    {
                        ServiceList.Clear();
                        IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                        var filter = Builders<BsonDocument>.Filter.Eq(r => r["serviceName"], textBox_serviceName.Text);
                        var update = Builders<BsonDocument>.Update.Set(r => r["servicePrice"], Convert.ToInt32(textBox_servicePrice.Text));
                        collection.UpdateOne(filter, update);
                        UpdateData();
                        isEditing = false;
                        Dialog.IsOpen = false;

                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(textBox_serviceName.Text) || String.IsNullOrEmpty(textBox_servicePrice.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                }
                else
                {
                    if (handler != null)
                    {
                        IMongoCollection<BsonDocument> collection = handler.GetCollection("Service");
                        List<BsonDocument> documents = collection.Find<BsonDocument>(new BsonDocument()).ToList();
                        foreach (BsonDocument service in documents)
                        {

                            if (service["serviceName"].AsString == textBox_serviceName.Text)
                            {
                                MessageBox.Show("Dịch vụ đã tồn tại.");
                            }
                        }
                        var newDoc = new BsonDocument
                        {
                            {"serviceName", textBox_serviceName.Text },
                            {"servicePrice", Convert.ToInt32(textBox_servicePrice.Text)},
                            {"serviceImage", image_serviceImage.Source.ToString() }
                        };
                        collection.InsertOne(newDoc);
                        UpdateData();
                        Dialog.IsOpen = false;
                    }
                }
            }

        }
        private static bool IsTextAllowed(string text)
        {
            return !StaticEvents.StaticEventHandler._regex.IsMatch(text);
        }

        private void textBox_servicePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
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
