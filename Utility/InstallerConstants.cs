namespace SocinatorInstaller.Utility
{
    public class InstallerConstants
    {
        #region API Server Links
        public static string ApplicationName { get; set; } = "Social Dominator";
        public static string GetDefaultIntallationPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        //For DevelopeMentAPI
        public static Uri uri = new Uri(@"https://storage.googleapis.com/powerbrowser-bulids/Power-dev/power-dev-bulids/Power%20Browser%20Dev%20Installer.exe");
        //For LiveAPI
        //  public static Uri uri = new Uri(@"https://storage.googleapis.com/powerbrowser-bulids/Power-live/power-live-builds/Power%20Browser%20Installer.exe");
        #endregion
    }
}
