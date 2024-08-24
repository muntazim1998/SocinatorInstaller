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
    /// Interaction logic for DeveloperUI.xaml
    /// </summary>
    public partial class DeveloperUI : UserControl
    {
        private static DeveloperUI instance { get; set; }
        private static Window CurrentWindow { get; set; }
        public DeveloperUI()
        {
            InitializeComponent();
        }
        public DeveloperUI(Window window):this()
        {
            CurrentWindow = window;
        }
        public static DeveloperUI GetInstance(Window window)
        {
            return instance ?? (instance = new DeveloperUI(window));
        }
    }
}
