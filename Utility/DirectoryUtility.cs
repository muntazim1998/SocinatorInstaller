using System.IO;

namespace SocinatorInstaller.Utility
{
    public static class DirectoryUtility
    {
        public static void CreateDirectory(string Path)
        {
            try
            {
                if(!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }catch{ }
        }
    }
}
