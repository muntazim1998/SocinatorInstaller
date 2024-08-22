using Microsoft.Win32;
using SocinatorInstaller.Utility;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SocinatorInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int NxtButtonCount = 1;
        private int BackButtonCount = 1;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _defaultPath = InstallerConstants.GetDefaultIntallationPath;
        public string DefaultPath
        {
            get => _defaultPath;
            set
            {
                _defaultPath = value;
                OnPropertyChanged(nameof(DefaultPath));
            }
        }
        private double _step1Opacity = 1.0;

        public double Step1Opacity
        {
            get { return _step1Opacity; }
            set
            {
                if (_step1Opacity != value)
                {
                    _step1Opacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _step2Opacity = 0.5;

        public double Step2Opacity
        {
            get { return _step2Opacity; }
            set
            {
                if (_step2Opacity != value)
                {
                    _step2Opacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _step3Opacity = 0.5;

        public double Step3Opacity
        {
            get { return _step3Opacity; }
            set
            {
                if (_step3Opacity != value)
                {

                    _step3Opacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _step4Opacity = 0.5;

        public double Step4Opacity
        {
            get { return _step4Opacity; }
            set
            {
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
        Color color1 = (Color)ColorConverter.ConvertFromString("#B0CB0E");
        Color color2 = (Color)ColorConverter.ConvertFromString("#38BBC8");

       public LinearGradientBrush gradientBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0)
        };
       
        private double _cancelBtnOpacity = 1.0;

        public double CancelBtnOpacity
        {
            get { return _cancelBtnOpacity; }
            set
            {
                if (_cancelBtnOpacity != value)
                {
                    _cancelBtnOpacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _backBtnOpacity = 1.0;

        public double BackBtnOpacity
        {
            get { return _backBtnOpacity; }
            set
            {
                if (_backBtnOpacity != value)
                {
                    _backBtnOpacity = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _nextBtnOpacity = 1.0;

        public double NextBtnOpacity
        {
            get { return _nextBtnOpacity; }
            set
            {
                if (_nextBtnOpacity != value)
                {
                    _nextBtnOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private  void click_NextBtn(object sender, RoutedEventArgs e)
        {
            if (NxtButtonCount == 1)
            {
                StartGrid.Visibility = Visibility.Collapsed;
                DownloadOptionGrid.Visibility = Visibility.Visible;
                BackBtnBorder.Visibility = Visibility.Visible;
                Step1Width = 20;
                Step1Opacity = 0.5;
                Step2Width = 40;
                Step2Opacity = 1;
                NxtButtonCount++;
                BackButtonCount = 1;
            }
            else if (NxtButtonCount == 2)
            {
                var Created = DirectoryUtility.CreateDirectory(DefaultPath = $"{DefaultPath}\\{InstallerConstants.ApplicationName}");
                if (!Created)
                {
                    txt_message.Text = "Unable To Install At Selected Location,Please Select Another Location";
                    return;
                }
                DownloadOptionGrid.Visibility = Visibility.Collapsed;
                ConfirmationGrid.Visibility = Visibility.Visible;
                Step2Width = 20;
                Step2Opacity = 0.5;
                Step3Width = 40;
                Step3Opacity = 1;
                BackButtonCount = 2;
                NxtButtonCount++;
            }
            else if (NxtButtonCount == 3)
            {
                ConfirmationGrid.Visibility = Visibility.Collapsed;
                Step3Width = 20;
                Step3Opacity = 0.5;
                Step4Width = 40;
                Step4Opacity = 1;
                CancelBtnOpacity = 0.5;
                CancelBtn.IsEnabled = false;
                BackBtnOpacity = 0.5;
                BackBtn.IsEnabled = false;
                NextBtnOpacity = 0.5;
                btn_Next.IsEnabled = false;
                InstallingGrid.Visibility = Visibility.Visible;
                InstallSocinator();
         //       Task.Delay(35000);
                //LOGIC FOR INSTALLATION COMPLETE
                
                NxtButtonCount++;
            }


        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BackButtonCount == 2)
            {
                ConfirmationGrid.Visibility = Visibility.Collapsed;
                DownloadOptionGrid.Visibility = Visibility.Visible;
                Step3Width = 20;
                Step3Opacity = 0.5;
                Step2Width = 40;
                Step2Opacity = 1;
                NxtButtonCount = 2;
                BackButtonCount--;
            }
            else  if (BackButtonCount == 1)
            {
                DownloadOptionGrid.Visibility = Visibility.Collapsed;
                StartGrid.Visibility = Visibility.Visible;
                BackBtnBorder.Visibility = Visibility.Collapsed;
                Step1Width = 40;
                Step1Opacity = 1;
                Step2Width = 20;
                Step2Opacity = 0.5;
                NxtButtonCount=1;
                BackButtonCount=0;
            }
        }
        string filename = "";
        private void InstallSocinator()
        {
          Task.Factory.StartNew(async () =>
            {
                this.Dispatcher.Invoke(() => filename = $"{DefaultPath}\\{InstallerConstants.ApplicationName}.exe");
                try
                {
                    await Task.Delay(2000);
                    FileUtility.CreateFile(filename);
                    WebClient wc = new WebClient();
                    wc.DownloadFileAsync(InstallerConstants.uri, filename);
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
                }
                catch (Exception ex)
                {
                    txt_message.Text = "Failed to Download/Install.";
                }
            });
        }
        private async void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            await App.Current.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    installingProgressStyle.Value = e.ProgressPercentage;
                    if (installingProgressStyle.Value == installingProgressStyle.Maximum)
                    {

                    }
                }
                catch (Exception)
                {
                    installingProgressStyle.Value = 0;
                    //txt_message.Text = "Failed";
                    //btn_Retry.Visibility = Visibility.Visible;
                }
            });
        }
        private async void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            await App.Current.Dispatcher.InvokeAsync(async () =>
            {
                if (e.Error == null)
                {
                    await Task.Delay(1000);
                    InstallingGrid.Visibility = Visibility.Collapsed;
                    InstallationCompleteGrid.Visibility = Visibility.Visible;
                    CancelBtnBorder.Visibility = Visibility.Collapsed;
                    BackBtnBorder.Visibility = Visibility.Collapsed;
                    NextBtnBorder.Visibility = Visibility.Collapsed;
                    CloseBtnBorder.Visibility = Visibility.Visible;
                    buttonStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    //try
                    //{
                    //    txt_message.Text = "Download complete!";
                    //    txt_message.Text = "Preparing to install...";
                    //    txt_message.Text = "Installation is in progress. Please wait...";
                    //    IsUnzippingCompleted = false;

                    //    customProgressStyle.Value = 20;
                    //    RemoveDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Power");
                    //    var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Power";
                    //    if (Directory.Exists(dir))
                    //    {
                    //        Directory.Delete(dir, true);
                    //    }
                    //    //       ProcessUnzip();
                    //    //      await ExctractFile();
                    //    customProgressStyle.Value = 100;
                    //    IsUnzippingCompleted = true;
                    //    Uri filepath = new Uri(filename);

                    //    await CreateInstaller(filepath);
                    //    var installationData = checkInstalled("Power");
                    //    var installedVersion = installationData[2];
                    //    var filesource = System.Reflection.Assembly.GetEntryAssembly().Location;
                    //    var dest = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\PowerSoftware\Power-Browser\Application\" + installedVersion + @"\Installer\setup.exe";
                    //    var OldInstaller = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\PowerSoftware\Power-Browser\Application\" + installedVersion + @"\Installer\setup1.exe";
                    //    if (File.Exists(dest))
                    //    {
                    //        File.Move(dest, OldInstaller);
                    //        await Task.Delay(2000);
                    //        File.Delete(dest);
                    //    }

                    //    File.Copy(filesource, dest, true);
                    //    //createShortcut();
                    //    txt_message.Text = "Installed successfully";
                    //    if (IsLaunch)
                    //    {
                    //        var process = Process.Start(UninstallRegKeyPath);
                    //        await Task.Delay(2000);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{ }
                    //App.Current.Shutdown();
                }
                else
                {
                    //txt_message.Text = "Unable to download, please check your internet connection.";
                    //ShowMessageModal(true, "Unable to download, please check your internet connection.");
                    //btn_Retry.Visibility = Visibility.Visible;
                }
            });
        }

        private void click_CloseBtn(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
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
        private void ShowMessageBoxModel(bool isyes = false, string msg = "", bool isAsync = false)
        {
            CustomMessageBox msgBox = new CustomMessageBox(isyes, msg);
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
            window.ShowDialog();
        }

        private void click_Cancel(object sender, RoutedEventArgs e)
        {
            ShowMessageBoxModel(msg: "Are you sure you want to cancel the installation ?");
        }

        private void click_CancelBtn(object sender, RoutedEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DiskCostBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Browsebtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            var result = dialog.ShowDialog();
            if (result.Value)
            {
                DefaultPath = dialog.FolderName;
            }
        }

        private void installingProgressStyle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}