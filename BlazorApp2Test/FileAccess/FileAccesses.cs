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

        public async Task DeleteFile(string fileName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{_userService.GetUserId()}/{fileName}"
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception($"Error encountered on server: {e.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"Unknown encountered on server: {e.Message}");
            }
        }

        public async Task DeleteAllFiles()
        {
            // Iterate over the contents of the bucket and delete all objects.
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
            };

            try
            {
                ListObjectsV2Response response;

                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);
                    response.S3Objects
                        .ForEach(async obj => await _s3Client.DeleteObjectAsync(_bucketName, obj.Key));

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error deleting objects: {ex.Message}");
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

            return response.S3Objects
                    .Select(item => item.Key.Split('/'))
                    .Where(parts => parts.Length > 1)
                    .Select(parts => parts[1])
                    .ToList();
        }

        public string GeneratePreSignedURL(string fileName)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{_userService.GetUserId()}/{fileName}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(Helper.R2_URL_EXPIRE_TIME),
                ResponseHeaderOverrides = new ResponseHeaderOverrides
                {
                    ContentDisposition = $"attachment; filename=\"{fileName}\""
                }
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public string GetFileSize(string fName)
        {
            // var filePath = Path.Combine(Helper.UploadFolderPath, _userService.GetUserId().ToString());
            // filePath = Path.Combine(filePath, fName);
            // var fileInfo = new FileInfo(filePath);
            // if (fileInfo.Exists)
            // {
            //     // Size in bytes
            //     long bytes = fileInfo.Length;

            //     // Convert size to more readable format
            //     if (bytes < 1024)
            //     {
            //         return bytes + " Bytes";
            //     }
            //     else if (bytes < 1024 * 1024)
            //     {
            //         return (bytes / 1024.0).ToString("0.00") + " KB";
            //     }
            //     else if (bytes < 1024 * 1024 * 1024)
            //     {
            //         return (bytes / (1024.0 * 1024.0)).ToString("0.00") + " MB";
            //     }
            //     else
            //     {
            //         return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("0.00") + " GB";
            //     }
            // }
            // else
            // {
            //     throw new Exception("Something happened during get file info: " + fName);
            // }
            return "";
        }
    }
}