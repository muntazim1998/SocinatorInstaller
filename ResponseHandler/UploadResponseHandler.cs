using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocinatorInstaller.ResponseHandler
{
    public class UploadResponseHandler
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string UploadedResult { get; set; }
    }
}
