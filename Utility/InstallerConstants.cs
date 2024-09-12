using SocinatorInstaller.Enums;
using System.Reflection;

namespace SocinatorInstaller.Utility
{
    public class InstallerConstants
    {
        public static string GetLocalFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string GetDownloadNode(ConfigMode configMode) => $"{ApplicationName}\\{configMode}";
        public static string ApplicationName { get; set; } = "Socinator";
        public static string AssemblyName => Assembly.GetEntryAssembly().GetName().Name;
        public static bool IsCloudBucket { get; set; } = false;
        public static string InstallerFolder => $"{GetLocalFolder}\\{ApplicationName}Installer";
        public static string ConfirmationMessageForClosing { get; set; } = $"{ApplicationName} is running Do you want to close before uninstalling ?";
        public static string GetDefaultIntallationPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        public static string GetInstallerExe => Assembly.GetEntryAssembly().Location;
        #region API Server Links

        //For DevelopeMentAPI
        public static Uri uri { get; set; } = new Uri(@"https://storage.googleapis.com/powerbrowser-bulids/Power-dev/power-dev-bulids/Power%20Browser%20Dev%20Installer.exe");
        //For LiveAPI
        //  public static Uri uri = new Uri(@"https://storage.googleapis.com/powerbrowser-bulids/Power-live/power-live-builds/Power%20Browser%20Installer.exe");
        #endregion
    }
}
