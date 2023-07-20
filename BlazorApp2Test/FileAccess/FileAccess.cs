using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp2Test.FileAccess
{
    public class FileAccess
    {

        public IBrowserFile selectedFile { get; set; }

        public string? error { get; set; }

        public bool isUploaded { get; set; }


        public FileAccess() 
        {
            error = null;
            isUploaded = false;
        }

        public async Task UploadFile(IBrowserFile selectedFile)
        {
            try
            {
                // Reset the states

                if (selectedFile == null)
                {
                    error = "Please select a file.";
                    return;
                }

                // File size validation - 5MB limit in this example
                var maxFileSize = 1024 * 1024 * 5;
                if (selectedFile.Size > maxFileSize)
                {
                    error = "File size must not exceed 5MB.";
                    return;
                }

                var buffer = new byte[selectedFile.Size];
                await selectedFile.OpenReadStream(maxFileSize).ReadAsync(buffer);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadFile");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = Path.Combine(filePath, selectedFile.Name);
                await System.IO.File.WriteAllBytesAsync(filePath, buffer);

                // If the code has reached this point without returning, it was successful
                isUploaded = true;
            }
            catch (Exception e)
            {
                error = $"An error occurred while uploading the file: {e.Message}";
            }
        }
    }
}
