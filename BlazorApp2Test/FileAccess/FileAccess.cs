﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.IO;

namespace BlazorApp2Test.FileAccess
{
    public class FileAccess
    {
        public string? Error { get; set; }

        public bool IsUploaded { get; set; }

        public string? FilePath { get; set; }

        public string? FileName { get; set; }


        public async Task UploadFile(IBrowserFile selectedFile, bool Rep)
        {
            try
            {

                if (selectedFile == null)
                {
                    Error = "Please select a file.";
                    return;
                }

                // File size validation - 30MB limit in this example
                var maxFileSize = 1024 * 1024 * 30;
                if (selectedFile.Size > maxFileSize)
                {
                    Error = "File size must not exceed 30MB.";
                    return;
                }

                FilePath = Path.Combine(Directory.GetCurrentDirectory(), Helper.UploadFolderPath);
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                if (Rep)
                {
                    FileName = selectedFile.Name;
                }
                else
                {
                    FileName = selectedFile.Name;

                    foreach (var f in Directory.GetFiles(Helper.UploadFolderPath))
                    {
                        if (Path.GetFileName(f) == FileName)
                        {
                            char separator = '.';
                            string[] parts = FileName.Split(separator);
                            parts[parts.Length - 2] = parts[parts.Length - 2] + "(1)";
                            FileName = "";
                            foreach (var part in parts)
                            {
                                FileName += part;
                                FileName += ".";
                            }
                            FileName = FileName.Substring(0, FileName.Length - 1);
                            break;
                        }
                    }

                    int i = 2;

                    bool cont = true;

                    while (cont)
                    {
                        cont = false;
                        foreach(var f in Directory.GetFiles(Helper.UploadFolderPath))
                        {
                            if (Path.GetFileName(f) == FileName)
                            {
                                char separator = '.';
                                string[] parts = FileName.Split(separator);
                                
                                separator = '(';
                                string[] parts2 = parts[parts.Length - 2].Split(separator);
                                FileName = parts2[0] + '(' + i.ToString() + ")" + '.' + parts[parts.Length - 1];

                                cont = true;
                            }
                        }
                        i++;
                    }
                }
                
                FilePath = Path.Combine(FilePath, FileName);

                await using FileStream fs = new(FilePath, FileMode.Create);

                await selectedFile.OpenReadStream(selectedFile.Size).CopyToAsync(fs);

                fs.Close();

                // If the code has reached this point without returning, it was successful
                IsUploaded = true;
            }
            catch (Exception e)
            {
                Error = $"An error occurred while uploading the file: {e.Message}";
            }
        }

        public void DeleteFile(string fName)
        {
            if(fName != null)
            {
                var filePath = Path.Combine(Helper.UploadFolderPath, fName);
                File.Delete(filePath);
            }
        }
    }
}
