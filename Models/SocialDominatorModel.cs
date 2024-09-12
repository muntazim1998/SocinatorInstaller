using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocinatorInstaller.Models
{
    public class SocialDominatorModel
    {
        public string Version { get; set; }
        public string ConfigMode { get; set; }
        public string PublicKey { get; set; }
        public string StoragePath { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public string ProductName { get; set; } = "SocialDominator";
    }
}
