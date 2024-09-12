using Microsoft.VisualBasic;
using SocinatorInstaller.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Security.Cryptography;
using SocinatorInstaller.Models;
using FirebaseAdmin;
using FireSharp.Interfaces;
using FireSharp;
using Google.Apis.Storage.v1;
using Google.Apis.Auth.OAuth2;
using SocinatorInstaller.ResponseHandler;
using Firebase.Storage;
using ICSharpCode.SharpZipLib.Zip;

namespace SocinatorInstaller.Utility
{
    public class FireBaseHelper
    {
        private static FireBaseHelper instance;
        private readonly StorageService storageService;
        public static FireBaseHelper GetInstance => instance ?? (instance = new FireBaseHelper());
        public FireBaseHelper()
        {
            try
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(FileUtilities.ReadServiceAccountConfig())
                });
            }
            catch { }
        }
        public static IFirebaseConfig config { get; private set; } = FileUtilities.GetFirebaseConfig();
        public static FirebaseClient client { get; private set; } = new FirebaseClient(config);
        public async Task<T> GetTaskAsync<T>(string Path)
        {
            var response = await client.GetAsync(Path.Replace(".", "_"));
        //        await client.GetTaskAsync(Path.Replace(".", "_"));
            return response.ResultAs<T>();
        }
        public async Task<T> SetTaskAsync<T>(string Path, T DataModel)
        {
            var response = await client.SetAsync(Path.Replace(".", "_"), DataModel);
            return response.ResultAs<T>();
        }
        public async Task<T> PushTaskAsync<T>(string Path, T DataModel)
        {
            var response = await client.PushAsync(Path.Replace(".", "_"), DataModel);
            return response.ResultAs<T>();
        }
        public async Task<T> GetList<T>(string Path)
        {
            var response = await client.GetAsync(Path.Replace(".", "_"));
            return response.ResultAs<T>();
        }
        public async Task<bool> DownloadSetupAsync(string DataBasePath, string DownloadPath, ProgressBar progressBar, TextBlock status)
        {
            try
            {
                var Nodes = await GetList<Dictionary<string, SocialDominatorModel>>(DataBasePath);
                if (Nodes != null && Nodes.Count > 0)
                {
                    var model = Nodes.LastOrDefault().Value as SocialDominatorModel;
                    using (var httpClient = new HttpClient())
                    {
                        try
                        {
                            // Send a GET request to the download URL
                            var response = await httpClient.GetAsync(model.StoragePath, HttpCompletionOption.ResponseHeadersRead);

                            // Ensure the request was successful
                            response.EnsureSuccessStatusCode();
                            long? totalBytes = response.Content.Headers.ContentLength;
                            var localFilePath = $"{FileUtilities.GetDownloadPath(model.ConfigMode, model.Version)}\\{model.ProductName}_{model.Version.Replace(".", "_")}.zip";
                            // Read the response content as a stream
                            using (var responseStream = await response.Content.ReadAsStreamAsync())
                            {
                                // Save the content to a file
                                using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                                {
                                    byte[] buffer = new byte[8192];
                                    long totalBytesRead = 0;
                                    int bytesRead;
                                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                    {
                                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                                        totalBytesRead += bytesRead;
                                        if (totalBytes.HasValue)
                                        {
                                            double progress = (double)totalBytesRead / totalBytes.Value * 100;
                                            progressBar.Value = progress;
                                            status.Text = $"Downloading {InstallerConstants.ApplicationName}.... {progress:F0} %";
                                        }
                                    }
                                    //await responseStream.CopyToAsync(fileStream);
                                }
                            }
                            FileUtilities.SaveConfig(new ProductInfo(localFilePath, ConfigMode.Release.ToString()));
                            status.Text = $"Extracting files please wait...";
                            await UnzipFileAsync(localFilePath, model, DownloadPath);
                            return true;
                        }
                        catch (HttpRequestException httpEx)
                        {
                            Console.WriteLine($"HTTP request error: {httpEx.Message}");
                        }
                        catch (IOException ioEx)
                        {
                            Console.WriteLine($"IO error: {ioEx.Message}");
                        }
                        return false;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        private async Task UnzipFileAsync(string localFilePath, SocialDominatorModel model, string downloadPath)
        {
            try
            {
                // Obtain the password for the ZIP file
                var finalKey = await GetPassword(model.PublicKey.ToString());

                // Open the ZIP file
                using (var archive = new ZipArchive(File.OpenRead(localFilePath), ZipArchiveMode.Read))
                {
                    string innerZipFilePath = string.Empty;

                    // Extract each entry in the ZIP file
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Check if the entry is a file and not a directory
                        if (!entry.FullName.EndsWith("/"))
                        {
                            string fullPath = Path.Combine(downloadPath, entry.FullName);

                            // Ensure the directory exists
                            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                            // Extract the file
                            await Task.Run(() =>
                            {
                                using (var entryStream = entry.Open())
                                using (var fileStream = File.Create(fullPath))
                                {
                                    entryStream.CopyTo(fileStream);
                                }
                            });

                            // Check if the entry is another ZIP file inside
                            if (string.IsNullOrEmpty(innerZipFilePath) && entry.FullName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                            {
                                innerZipFilePath = fullPath;
                            }
                        }
                    }

                    // If there's an inner ZIP file, extract it too
                    if (!string.IsNullOrEmpty(innerZipFilePath))
                    {
                        await Task.Run(() => System.IO.Compression.ZipFile.ExtractToDirectory(innerZipFilePath, downloadPath));
                        FileUtilities.DeleteFile(innerZipFilePath);
                    }
                }

                // Further processing if MSI files are found in the extracted contents
                await ExtractMSIIFFound(downloadPath);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Clean up the initial downloaded ZIP file
                FileUtilities.DeleteFile(localFilePath);
            }
        }

        private async Task ExtractMSIIFFound(string downloadPath)
        {
            try
            {
                var files = Directory.GetFiles(downloadPath).ToList();
                if (files != null && files.Count > 0 && files.Any(x => Path.GetExtension(x) == ".msi"))
                {
                    var targetFile = files.FirstOrDefault(x => Path.GetExtension(x) == ".msi");
                    var targetFileName = Path.GetFileName(targetFile);
                    var destinationFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{targetFileName}";
                    File.Move(targetFile, destinationFile);
                    foreach (var file in files.ToList())
                    {
                        FileUtilities.DeleteFile(file);
                    }
                    try
                    {
                        var processStartInfo = new ProcessStartInfo
                        {
                            FileName = "msiexec.exe",
                            Arguments = $"/a \"{destinationFile}\" /qn TARGETDIR=\"{downloadPath}\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (var process = Process.Start(processStartInfo))
                        {
                            process.WaitForExit();
                            if (process.ExitCode != 0)
                            {
                                throw new InvalidOperationException($"msiexec failed with exit code {process.ExitCode}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        FileUtilities.DeleteFile(destinationFile);
                    }
                }
            }
            catch { }
        }

        public async Task<UploadResponseHandler> UploadFileAsync(string FilePath, string BucketName, string FileName)
        {
            try
            {
                var storage = new FirebaseStorage(BucketName);
                var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                var sref = await storage.Child(FileName).PutAsync(stream);
                var downloadUrl = await storage.Child(FileName?.Trim())
                    .GetDownloadUrlAsync();
                return new UploadResponseHandler { UploadedResult = downloadUrl, Success = true, ErrorMessage = string.Empty };
            }

            catch (Exception ex)
            {
                return new UploadResponseHandler { UploadedResult = string.Empty, Success = false, ErrorMessage = string.Empty }; ;
            }
        }
        public async Task<UploadResponseHandler> UploadSetupFile(string FilePath, ConfigMode configMode = ConfigMode.Release, bool ProtectFile = true)
        {
            try
            {
                var cred = FileUtilities.GetAuthCred();
                var protectedFile = await GetProtectedFile(FilePath, ProtectFile, configMode);
                var productInfo = new ProductInfo(protectedFile.Item1, configMode.ToString());
                var uploadResponse = await UploadFileAsync(protectedFile.Item1, cred.BucketName, productInfo.FileName);
                if (uploadResponse != null && uploadResponse.Success)
                {
                    var UpdateData = new SocialDominatorModel
                    {
                        ProductName = productInfo.ProductName,
                        Version = productInfo.ProductVersion,
                        ConfigMode = productInfo.ProductConfig.ToString(),
                        PublicKey = protectedFile.Item2,
                        ReleaseDate = DateTime.Now,
                        StoragePath = uploadResponse.UploadedResult
                    };
                    var uploaded = await SetTaskAsync<SocialDominatorModel>($"{productInfo.ProductName}/{productInfo.ProductConfig}/{productInfo.ProductVersion}", UpdateData);
                    FileUtilities.SaveConfig(productInfo);
                    FileUtilities.DeleteFile(protectedFile.Item1);
                }
                return uploadResponse;
            }
            catch (Exception ex) { return new UploadResponseHandler { UploadedResult = string.Empty, Success = false, ErrorMessage = ex.Message }; }
        }

      
        private async Task<(string, string)> GetProtectedFile(string filePath, bool protectFile, ConfigMode configMode)
        {
            var password = string.Empty;
            try
            {
                var fileInfo = new FileInfo(filePath);
                var productInfo = new ProductInfo(filePath, configMode.ToString());
                var outputFile = $"{FileUtilities.GetCurrentDirectory}\\{fileInfo.Name}";

                // Creating a zip file stream
                using (FileStream zipFileStream = new FileStream(outputFile, FileMode.Create))
                using (ZipOutputStream zipOutputStream = new ZipOutputStream(zipFileStream))
                {
                    zipOutputStream.SetLevel(9); // Set compression level

                    // Setting password if required
                    if (protectFile)
                    {
                        var keyData = await SetPassword(productInfo.ProductVersion);
                        password = keyData.Item2;
                        zipOutputStream.Password = keyData.Item1; // Setting the password for encryption
                    }

                    fileInfo = new FileInfo(filePath); // Reloading file info in case of changes
                    string entryName = fileInfo.Name;  // Entry name in the ZIP
                    ZipEntry entry = new ZipEntry(entryName)
                    {
                        DateTime = fileInfo.LastWriteTime,
                        Size = fileInfo.Length
                    };

                    zipOutputStream.PutNextEntry(entry); // Add the entry to the zip stream

                    // Copying the file content to the zip stream
                    using (FileStream fileStream = fileInfo.OpenRead())
                    {
                        await fileStream.CopyToAsync(zipOutputStream);
                    }

                    zipOutputStream.CloseEntry();
                    zipOutputStream.Close();
                    zipOutputStream.Dispose();
                }

                // Return the path of the created zip file and the protection key if any
                return (outputFile, password);
            }
            catch
            {
                // In case of failure, return the original file path and an empty password
                return (filePath, password);
            }
        }


        private async Task<(string, string)> SetPassword(string version)
        {
            var data = string.Empty;
            try
            {
                var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{InstallerConstants.ApplicationName}_{version.Replace(".", "_")}"));
                using (SHA256 sHA = SHA256.Create())
                {
                    byte[] bytes = sHA.ComputeHash(Encoding.UTF8.GetBytes(key));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return (builder.ToString(), Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString())))));
                }
            }
            catch { return (data, ""); }
        }
        private async Task<string> GetPassword(string publicKey)
        {
            var data = string.Empty;
            try
            {
                var key = Convert.FromBase64String(publicKey);
                return Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(key)));
            }
            catch { return data; }
        }
    }
}
