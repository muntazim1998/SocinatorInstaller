using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocinatorInstaller.Models
{
    public class FireBaseAuthCred
    {
        public string AuthSecret { get; set; }
        public string BasePath { get; set; }
        public string BucketName { get; set; }
    }
}
