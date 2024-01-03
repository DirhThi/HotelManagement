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

using MongoDB.Bson;
using MongoDB.Driver;

namespace Hotel_Management.Pages.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public class Account
    {
        public string accountId { get; set; }
        public string accountPassword { get; set;}
    }

    

    public partial class Login : Page
    {
        List<Account> usersList = new List<Account>();
        public Login()
        {
            InitializeComponent();
            MongoClient client = new MongoClient("mongodb+srv://vitalis:arthur010203@cluster0.4opqwlz.mongodb.net/");
            IMongoDatabase database = client.GetDatabase("HotelManagement");
            IMongoCollection<BsonDocument> collectionUser = database.GetCollection<BsonDocument>("User");
            LayUsers(collectionUser);
        }

        public void LayUsers(IMongoCollection<BsonDocument> collectionUser)
        {
            List<BsonDocument> documentsUser = collectionUser.Find(new BsonDocument()).ToList();
            string accountId;
            string accountPassword;
            usersList.Clear();
            foreach (BsonDocument user  in documentsUser) 
            {
                accountId = user["accountId"].AsString;
                accountPassword = user["accountPassword"].AsString;
                usersList.Add(new Account { accountId = accountId, accountPassword = accountPassword});
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginScreen.NavigationService.Navigate(new System.Uri("Pages/Navigator/Navigator.xaml", UriKind.RelativeOrAbsolute));

            //Bỏ comment phía dưới VÀ comment dòng trên để test

            /*
            if(string.IsNullOrEmpty(id.Text) || string.IsNullOrEmpty(pw.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu !", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                id.Clear();
                pw.Clear();
            }
            else
            {
                int idIndex = usersList.FindIndex(x => x.accountId == id.Text);
                int pwIndex = usersList.FindIndex(x => x.accountPassword == pw.Password.ToString());
                if(idIndex == -1 || pwIndex == -1)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác. Vui lòng nhập lại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    id.Clear();
                    pw.Clear();
                }
                else
                {
                    if(idIndex == pwIndex)
                    {
                        LoginScreen.NavigationService.Navigate(new System.Uri("Pages/Navigator/Navigator.xaml", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác. Vui lòng nhập lại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        id.Clear();
                        pw.Clear();
                    }
                }
            }
            */
        }
    }
}
