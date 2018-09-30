using LoveApp.Model;
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

namespace LoveApp.View
{
    /// <summary>
    /// Interaction logic for Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public static int userId = -1;
        public static string appId = "";
        public static Model1 db = new Model1();
        public Autorization()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Users us = new Users();
            us = db.Users.FirstOrDefault(f => f.email == EmailAuorizationTB.Text && f.password == PasswordAutorizatonTB.Password);
            if(us == null)
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
            else
            {
                MainWindow.mf.Source = new Uri("View/MainPage.xaml", UriKind.RelativeOrAbsolute);
                userId = us.user_id;
                Random rnd = new Random();
                for(int i = 0; i<10; i++)
                {
                    char c = (char)rnd.Next(48, 122);
                    appId += c;
                }
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mf.Source = new Uri("View/RegistrationPage.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
