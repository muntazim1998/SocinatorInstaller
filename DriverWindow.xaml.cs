using System.Windows;
using System.Windows.Controls;

namespace SocinatorInstaller
{
    /// <summary>
    /// Interaction logic for DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        public int Mode = 0;//Mode is 0 for Installer UI and Mode is 1 for Developer UI.
        public UserControl SelectedUserControl {  get; set; }=new UserControl();
        public DriverWindow()
        {
            InitializeUserControl();
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeUserControl()
        {
//#if DEBUG
//            Mode = 1;
//#endif
            SelectedUserControl = Mode== 0 ? MainWindow.GetInstance(this):
                DeveloperUI.GetInstance(this);
        }
    }
}
