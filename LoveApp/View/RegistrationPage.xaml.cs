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

namespace LoveApp.Images
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public static Model1 db = new Model1();
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void FullRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Users user = new Users();
            if(!string.IsNullOrEmpty(NameRegistrationTB.Text))
            {
                NameRegistrationLabel.Foreground = new SolidColorBrush(Colors.Black);

                if(!string.IsNullOrEmpty(PhoneRegistrationTB.Text))
                {
                    PhoneRegistrationLabel.Foreground = new SolidColorBrush(Colors.Black);
                   if(!string.IsNullOrEmpty(EmailRegistrationTB.Text))
                    {
                        EmailRegistrationLabel.Foreground = new SolidColorBrush(Colors.Black);
                        user.name = NameRegistrationTB.Text;
                        user.phone = PhoneRegistrationTB.Text;
                        user.email = EmailRegistrationTB.Text;
                        Random rand = new Random();
                        string pass = "";
                        for(int i = 0; i<12; i++)
                        {
                            char c = (char)rand.Next(48, 122);
                            pass += c;
                        }
                        user.password = pass;
                        try
                        {
                            db.Users.Add(user);
                            db.SaveChanges();
                            MessageBox.Show("Регистрация выполнена.\n Ваш пароль - "+pass);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                       
                    }
                   else
                    {
                        EmailRegistrationLabel.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
                else
                {
                    PhoneRegistrationLabel.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                NameRegistrationLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}

