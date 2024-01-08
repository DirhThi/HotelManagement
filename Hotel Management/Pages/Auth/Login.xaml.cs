using Hotel_Management.MongoDatabase;
using MongoDB.Bson;
using MongoDB.Driver;
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

namespace Hotel_Management.Pages.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    public partial class Login : Page
    {
        public static User currentUser;
        public Login()
        {
            InitializeComponent();
            button_Login.IsDefault = true;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            IMongoCollection<BsonDocument> userCollection = MongoHandler.GetInstance().GetCollection("User");
            var filterAccountId= Builders<BsonDocument>.Filter.Eq("accountId",id.Text);
            BsonDocument user = userCollection.Find(filterAccountId).FirstOrDefault();
            if (user == null) 
            {
                MessageBox.Show("Tên tài khoản không chính xác!");
            }
            else
            {
                if (pw.Password == user["accountPassword"].AsString)
                {
                    currentUser = new User() {
                        UserName = user["userName"].AsString,
                        DateOfBirth = user["dateOfBirth"].ToUniversalTime(),
                        PhoneNumber = user["phoneNumber"].AsString,
                        IdNumber = user["idNumber"].AsString,
                        Email = user["email"].AsString,
                        UserRole= user["userRole"].AsString,
                        AccoutId= user["accountId"].AsString,
                        AccountPassword= user["accountPassword"].AsString
                    };
                    LoginScreen.NavigationService.Navigate(new System.Uri("Pages/Navigator/Navigator.xaml" , UriKind.RelativeOrAbsolute));
                }
                else
                {
                    MessageBox.Show("Mật khẩu không chính xác!");
                }
            }
            
        }
    }
    public class User
    {
        public string UserName { get; set;}
        public DateTime DateOfBirth { get; set;} 
        public string PhoneNumber { get; set;}
        public string IdNumber { get; set;}
        public string Email { get; set;}
        public string UserRole { get;set; }
        public string AccoutId { get; set;}
        public string AccountPassword { get; set;}
    } 
}
