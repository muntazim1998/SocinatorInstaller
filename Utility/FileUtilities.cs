using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SocinatorInstaller.Models;
using System.IO;
using System.Reflection;

namespace SocinatorInstaller.Utility
{
    public class FileUtilities
    {
        public static bool CreateFile(string FilePath)
        {
            try
            {
                if (!File.Exists(FilePath))
                    File.Create(FilePath).Close();
                return true;
            }
            catch { return false; }
        }
        public static string GetTempInstallationFile()
        {
            DirectoryUtility.CreateDirectory(InstallerConstants.InstallerFolder);
            return $"{InstallerConstants.InstallerFolder}\\{InstallerConstants.ApplicationName}.zip";
        }
        public static string GetCurrentDirectory => Directory.GetCurrentDirectory();
        public static FireBaseAuthCred GetAuthCred()
        {
            try
            {
                return JsonConvert.DeserializeObject<FireBaseAuthCred>(ReadResource($"SocialDominatorInstaller.FireBaseConfigGLB.json"));
            }
            catch { return new FireBaseAuthCred(); }
        }
        public static string ReadResource(string ResourceName)
        {
            var json = string.Empty;
            Stream stream = null;
            try
            {
                using (stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }
            }
            catch { }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
            return json;
        }
        public static IFirebaseConfig GetFirebaseConfig()
        {
            var config = GetAuthCred();
            return new FirebaseConfig
            {
                AuthSecret = config.AuthSecret,
                BasePath = config.BasePath
            };
        }
        public static string ReadServiceAccountConfig()
        {
            var json = string.Empty;
            try
            {
                json = ReadResource($"SocialDominatorInstaller.ServiceAccountGLB.json");
            }
            catch { }
            return json;
        }
        public static string GetServiceAccountFileName => $"{GetCurrentDirectory}/ServiceAccountGLB.json";
        public static void SaveConfig(ProductInfo productInfo)
        {
            try
            {
                File.WriteAllText($"{GetCurrentDirectory}/{productInfo.ProductConfig}.txt", JsonConvert.SerializeObject(productInfo));
            }
            catch { }
        }
        public static ProductInfo GetConfig(string Config)
        {
            try
            {
                return JsonConvert.DeserializeObject<ProductInfo>(File.ReadAllText($"{GetCurrentDirectory}/{Config}.txt"));
            }
            catch { return new ProductInfo(); }
        }
        public static string GetDownloadPath(string Config, string Version)
        {
            var downloadPath = Path.Combine(GetCurrentDirectory, InstallerConstants.ApplicationName);
            DirectoryUtility.CreateDirectory(downloadPath);
            downloadPath = Path.Combine(downloadPath, Config);
            DirectoryUtility.CreateDirectory(downloadPath);
            downloadPath = Path.Combine(downloadPath, $"{InstallerConstants.ApplicationName}_{Version.Replace(".", "_")}");
            DirectoryUtility.CreateDirectory(downloadPath);
            return downloadPath;
        }
        public static bool DeleteFile(string FilePath)
        {
            try
            {
                File.Delete(FilePath);
                return true;
            }
            catch { return false; }
        }
        public static bool CopyFiles(string source, string dest, bool Overwrite = true)
        {
            try
            {
                File.Copy(source, dest, Overwrite);
                return true;
            }
            catch { return false; }
        }

        public static void CopyInstaller(string dest)
        {
            try
            {
                using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SocialDominatorInstaller.UnInstaller.exe"))
                {
                    if (resourceStream != null)
                    {
                        using (var fileStream = new FileStream(dest, FileMode.Create, FileAccess.Write))
                        {
                            resourceStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
