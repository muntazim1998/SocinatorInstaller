using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool isLaunch=true;
        string[] installedInfo = new string[3];
        string appName="Social Dominator";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _step1Opacity=1.0;

        public double Step1Opacity
        {
            get { return _step1Opacity; }
            set {
                if (_step1Opacity != value)
                {
                    _step1Opacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _step2Opacity =0.5;

        public double Step2Opacity
        {
            get { return _step2Opacity; }
            set {
                if (_step2Opacity != value)
                {
                    _step2Opacity = value;
                    OnPropertyChanged();
                }
                }
            }
        private double _step3Opacity=0.5;

        public double Step3Opacity
        {
            get { return _step3Opacity; }
            set {
                if (_step3Opacity != value)
                {

                    _step3Opacity = value;
                    OnPropertyChanged();
                }
                }
            }
        private double _step4Opacity=0.5;

        public double Step4Opacity
        {
            get { return _step4Opacity; }
            set {
                if (_step4Opacity != value)
                {
                    _step4Opacity = value;
                    OnPropertyChanged();
                }
                }
            }
        private int step1Width = 40;

        public int Step1Width
        {
            get { return step1Width; }
            set { step1Width = value; OnPropertyChanged(); }
        }
        private int step2Width = 20;

        public int Step2Width
        {
            get { return step2Width; }
            set { step2Width = value; OnPropertyChanged(); }
        }
        
        private int step3Width = 20;

        public int Step3Width
        {
            get { return step3Width; }
            set { step3Width = value; OnPropertyChanged(); }
        }
        
        private int step4Width = 20;

        public int Step4Width
        {
            get { return step4Width; }
            set { step4Width = value; OnPropertyChanged(); }
        }
        Color color1= (Color)ColorConverter.ConvertFromString("#B0CB0E");
        Color color2= (Color)ColorConverter.ConvertFromString("#38BBC8");

        LinearGradientBrush gradientBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0)
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            gradientBrush.GradientStops.Add(new GradientStop(color1, 0.0));
            gradientBrush.GradientStops.Add(new GradientStop(color2, 1.0));

            // Apply the brush to the Rectangle's Fill
            gradientRectangle1.Fill = gradientBrush;
            gradientRectangle2.Fill = gradientBrush;
            gradientRectangle3.Fill = gradientBrush;
            gradientRectangle4.Fill = gradientBrush;

        }

        private void click_NextBtn(object sender, RoutedEventArgs e)
        {
            Step1Width = 20;
            Step1Opacity = 0.5;
            Step2Width = 40;
            Step2Opacity = 1;
            BackBtnBorder.Visibility = Visibility.Visible;
            StartGrid.Visibility = Visibility.Collapsed;
            DownloaderGrid.Visibility = Visibility.Visible;
        }

        private void click_CloseBtn(object sender, RoutedEventArgs e)
        {
            ShowMessageBoxModel();
        }

        private void click_UpgradeBtn(object sender, RoutedEventArgs e)
        {

        }

        

        private void click_BackBtn(object sender, RoutedEventArgs e)
        {

        }

        private void chk_checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_checked_lauch(object sender, RoutedEventArgs e)
        {

        }

        private void chk_unchecked_launch(object sender, RoutedEventArgs e)
        {

        }

        private void click_ButtonAccptInstall(object sender, RoutedEventArgs e)
        {

        }

        private void click_Uninstall(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }
        private void ShowMessageBoxModel(bool isyes=false,string msg="",bool isAsync=false)
        {
            CustomMessageBox msgBox=new CustomMessageBox(isyes,msg);
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 50, 300, 108);
            rect.RadiusX = 10;
            rect.RadiusY = 10;
            Window window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                BorderThickness = new Thickness(0),
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None,
                Height = 160,
                Width = 300,
                Background = Brushes.Transparent,
                Clip = rect
            };
            msgBox.MyWindow = window;
            window.Content = msgBox;
            if (isAsync)
                window.ShowDialog();
            else
                window.Show();
        }

        private void click_Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void click_CancelBtn(object sender, RoutedEventArgs e)
        {

        }
    }
}