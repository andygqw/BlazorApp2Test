using Microsoft.AspNetCore.Components.Forms;
using System.IO;

namespace BlazorApp2Test.FileAccess
{
    public class FileAccess
    {
        public string? Error { get; set; }

        public bool IsUploaded { get; set; }

        public string? FilePath { get; set; }

        public string? FileName { get; set; }


        public async Task UploadFile(IBrowserFile selectedFile)
        {
            try
            {

                if (selectedFile == null)
                {
                    Error = "Please select a file.";
                    return;
                }

                // File size validation - 5MB limit in this example
                var maxFileSize = 1024 * 1024 * 5;
                if (selectedFile.Size > maxFileSize)
                {
                    Error = "File size must not exceed 5MB.";
                    return;
                }

                FilePath = Path.Combine(Directory.GetCurrentDirectory(), Helper.UploadFolderPath);
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                FileName = selectedFile.Name;
                FilePath = Path.Combine(FilePath, selectedFile.Name);

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
