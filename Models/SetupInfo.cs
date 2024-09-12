using CommunityToolkit.Mvvm.ComponentModel;
using SocinatorInstaller.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocinatorInstaller.Models
{
    public partial class SetupInfo : ObservableObject
    {
        [ObservableProperty]
        public string setupPath;
        [ObservableProperty]
        public string title = "Upload Setup To Bucket";
        [ObservableProperty]
        public bool progressEnable = false;
        [ObservableProperty]
        public string status;
        [ObservableProperty]
        public List<ConfigMode> config = new List<ConfigMode> { ConfigMode.Release, ConfigMode.Debug, ConfigMode.Test, ConfigMode.Custom };
        [ObservableProperty]
        public ConfigMode _selectedConfig = ConfigMode.Release;
    }
}
