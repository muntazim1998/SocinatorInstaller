using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocinatorInstaller.Utility
{
    public class InstalledInfo
    {
        public string DisplayName { get; set; }
        public string UnInstallString { get; set; }
        public string Version { get; set; }
        public bool IsInstalled { get; set; }
    }
}
