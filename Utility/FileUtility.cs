using System.IO;

namespace SocinatorInstaller.Utility
{
    public static class FileUtility
    {
        public static bool CreateFile(string FilePath)
        {
            try
            {
                if(!File.Exists(FilePath))
                    File.Create(FilePath).Close();
                return true;
            }
            catch { return false; }
        }
    }
}
