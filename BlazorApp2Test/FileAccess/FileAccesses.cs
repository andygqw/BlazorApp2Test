using BlazorApp2Test.Components;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BlazorApp2Test.Data;

namespace BlazorApp2Test.FileAccess
{
    public class FileAccesses
    {
        private readonly UserService _userService;
        private readonly LogData _logData;
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public FileAccesses(UserService s, LogData l, string accessKey, string secretKey, string serviceUrl, string bucketName )
        {
            _userService = s;
            _logData = l;
            
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
                DisablePayloadSigning = true// Required for Cloudflare R2
            };
            await _s3Client.PutObjectAsync(putRequest);
            await _logData.Log("Upload file", key, id);
        }

        public async Task DeleteFile(string fileName)
        {
            try
            {
                var id = _userService.GetUserId();
                var key = $"{id}/{fileName}";
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                await _logData.Log("Delete file", key, id);
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
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{_userService.GetUserId()}/"
            };
            var response = await _s3Client.ListObjectsV2Async(request);

            if (response.S3Objects.Count == 0)
            {
                throw new Exception("No files found to delete");
            }

            var deleteObjectsRequest = new DeleteObjectsRequest
            {
                BucketName = _bucketName
            };

            foreach (var s3Object in response.S3Objects)
            {
                deleteObjectsRequest.AddKey(s3Object.Key);
            }

            await _s3Client.DeleteObjectsAsync(deleteObjectsRequest);
        }

        public async Task<List<S3Object>> ListUserFilesAsync()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{_userService.GetUserId()}/"
            };
            var response = await _s3Client.ListObjectsV2Async(request);

            return response.S3Objects.Select(item => new S3Object
                    {
                        Key = Path.GetFileName(item.Key),
                        Size = item.Size
                    }).ToList();
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

        public async Task UploadMemoImg(Stream fileStream, string fileName)
        {
            var key = $"{Helper.R2_MEMO_FOLDER}/{_userService.GetUserId()}/{fileName}";

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                DisablePayloadSigning = true// Required for Cloudflare R2
            };
            await _s3Client.PutObjectAsync(putRequest);
        }

        public async Task DeleteMemoImg(string fileName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{Helper.R2_MEMO_FOLDER}/{_userService.GetUserId()}/{fileName}"
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

        public string GeneratePreSignedURLForMemoImg(string fileName, int id)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{Helper.R2_MEMO_FOLDER}/{id}/{fileName}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(Helper.R2_URL_EXPIRE_TIME)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public string GetFileSize(long bytes)
        {
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
    }
}