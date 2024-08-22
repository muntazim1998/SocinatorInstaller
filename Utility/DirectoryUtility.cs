using System.IO;

namespace SocinatorInstaller.Utility
{
    public static class DirectoryUtility
    {
        public static bool CreateDirectory(string Path)
        {
            try
            {
                if(!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
                return true;
            }catch{ return false; }
        }
    }
}
