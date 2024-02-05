using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.IO;

namespace BlazorApp2Test.FileAccess
{
    internal class FileAccesses
    {
        public FileAccesses()
        {

        }

        public async Task<int> UploadFile(IBrowserFile selectedFile, bool Rep)
        {
            if (selectedFile == null)
            {
                throw new ArgumentException("Selected file does not exist");
            }

            if (selectedFile.Size > Helper.FileMaxSize)
            {
                throw new IOException("Selected file exceeds limit of " + Helper.FileMaxSize / (1024 * 1024) + "MB");
            }

            if (selectedFile.Size == 0)
            {
                throw new IOException("Something wrong uploading file to the server: 0 byte");
            }

            // Check file path exists
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), Helper.UploadFolderPath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fileName = selectedFile.Name;

            if (!Rep)
            {
                if (fileName.Contains("."))
                {

                    foreach (var f in Directory.GetFiles(Helper.UploadFolderPath))
                    {
                        if (Path.GetFileName(f) == fileName)
                        {
                            char separator = '.';
                            string[] parts = fileName.Split(separator);
                            parts[parts.Length - 2] = parts[parts.Length - 2] + "(1)";
                            fileName = "";
                            foreach (var part in parts)
                            {
                                fileName += part;
                                fileName += ".";
                            }
                            fileName = fileName.Substring(0, fileName.Length - 1);
                            break;
                        }
                    }

                    int i = 2;

                    bool cont = true;

                    while (cont)
                    {
                        cont = false;
                        foreach (var f in Directory.GetFiles(Helper.UploadFolderPath))
                        {
                            if (Path.GetFileName(f) == fileName)
                            {
                                char separator = '.';
                                string[] parts = fileName.Split(separator);

                                separator = '(';
                                string[] parts2 = parts[parts.Length - 2].Split(separator);
                                parts[parts.Length - 2] = parts2[0] + "(" + i.ToString() + ")";
                                fileName = "";
                                foreach (var part in parts)
                                {
                                    fileName += part;
                                    fileName += ".";
                                }
                                fileName = fileName.Substring(0, fileName.Length - 1);

                                cont = true;
                            }
                        }
                        i++;
                    }
                }
                else
                {

                    int i = 2;

                    bool cont = true;

                    while (cont)
                    {
                        cont = false;
                        foreach (var f in Directory.GetFiles(Helper.UploadFolderPath))
                        {
                            if (Path.GetFileName(f) == fileName)
                            {
                                char separator = '(';
                                string[] parts = fileName.Split(separator);
                                if (parts.Length == 1)
                                {
                                    fileName += "(1)";
                                }
                                else
                                {
                                    parts[0] += "(" + i.ToString() + ")";
                                    fileName = parts[0];
                                }

                                cont = true;
                            }
                        }
                        i++;
                    }
                }
            }

            filePath = Path.Combine(filePath, fileName);

            await using FileStream fs = new(filePath, FileMode.Create);

            await selectedFile.OpenReadStream(selectedFile.Size).CopyToAsync(fs);

            fs.Close();

            return Helper.ReturnGood;
        }

        public void DeleteFile(string fName)
        {
            if(fName != null)
            {
                var filePath = Path.Combine(Helper.UploadFolderPath, fName);
                System.IO.File.Delete(filePath);
            }
            else
            {
                throw new Exception("File name doesn exist in current directory");
            }
        }

        public void DeleteAllFiles()
        {
            foreach (var file in Directory.GetFiles(Helper.UploadFolderPath))
            {
                DeleteFile(Path.GetFileName(file));
            }
        }

        public List<string> GetAllFileNames()
        {
            List<string> files = new List<string>();

            foreach (var file in Directory.GetFiles(Helper.UploadFolderPath))
            {
                files.Add(Path.GetFileName(file));
            }

            return files;
        }

        public string GetFileSize(string fName)
        {
            var filePath = Path.Combine(Helper.UploadFolderPath, fName);
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                // Size in bytes
                long bytes = fileInfo.Length;

                // Convert size to more readable format
                if (bytes < 1024)
                {
                    return bytes + " Bytes";
                }
                else if (bytes < 1024 * 1024)
                {
                    return (bytes / 1024.0).ToString("0.00") + " KB";
                }
                else if (bytes < 1024 * 1024 * 1024)
                {
                    return (bytes / (1024.0 * 1024.0)).ToString("0.00") + " MB";
                }
                else
                {
                    return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("0.00") + " GB";
                }
            }
            else
            {
                throw new Exception("Something happened during get file info: " + fName);
            }
        }
    }
}