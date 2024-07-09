using BlazorApp2Test.Components;
using BlazorApp2Test.Data;
using Microsoft.AspNetCore.Components.Forms;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorApp2Test.FileAccess
{
    internal class FileAccesses
    {
        private readonly UserDbContext _context;
        private readonly UserService _userService;
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public FileAccesses(UserDbContext context, UserService s, string accessKey, string secretKey, string serviceUrl, string bucketName )
        {
            _context = context;
            _userService = s;
            
            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl
            };
            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
            _bucketName = bucketName;
        }

        public async Task UploadFile(Stream fileStream, string fileName)
        {
            var id = _userService.GetUserId();
            var key = $"{id}/{fileName}"; 

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                DisablePayloadSigning = true
            };
            await _s3Client.PutObjectAsync(putRequest);
        }

        public void DeleteFile(string fName)
        {
            if(fName != null)
            {
                var filePath = Path.Combine(Helper.UploadFolderPath,
                                        _userService.GetUserId().ToString());
                filePath = Path.Combine(filePath, fName);
                System.IO.File.Delete(filePath);
            }
            else
            {
                throw new Exception("File name doesn exist in current directory");
            }
        }

        public void DeleteAllFiles()
        {
            var filePath = Path.Combine(Helper.UploadFolderPath,
                                        _userService.GetUserId().ToString());
            foreach (var file in Directory.GetFiles(filePath))
            {
                DeleteFile(Path.GetFileName(file));
            }
        }

        public async Task<List<String>> ListUserFilesAsync()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{_userService.GetUserId()}/"
            };
            var response = await _s3Client.ListObjectsV2Async(request);

            return response.S3Objects.Select(item => item.Key).ToList();
        }

        public string GeneratePreSignedURL(string fileName)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public string GetFileSize(string fName)
        {
            var filePath = Path.Combine(Helper.UploadFolderPath, _userService.GetUserId().ToString());
            filePath = Path.Combine(filePath, fName);
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