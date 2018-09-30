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

namespace LoveApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame mf;
        public MainWindow()
        {
            InitializeComponent();
            mf = MainFrame;
            mf.Source = new Uri("View/Autorization.xaml", UriKind.RelativeOrAbsolute);
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("Images/Autorization.jpg", UriKind.RelativeOrAbsolute));
            myBrush.ImageSource = image.Source;
            mf.Background = myBrush;
        }
    }
}
