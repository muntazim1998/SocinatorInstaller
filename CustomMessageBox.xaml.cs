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

namespace SocinatorInstaller
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : UserControl
    {
        public Window MyWindow { get; set; }
        public CustomMessageBox(bool isYes=false,string msg="")
        {
            InitializeComponent();
            if (isYes)
            {
                btnNo.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Collapsed;
                btnOk.Visibility = Visibility.Visible;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                txt_Message.Text = msg;
            }
        }

        private void clk_Yes(object sender, RoutedEventArgs e)
        {
            MyWindow.Close();
            App.Current.Shutdown();
        }

        private void clk_No(object sender, RoutedEventArgs e)
        {
            MyWindow.Close();
        }
    }
}
