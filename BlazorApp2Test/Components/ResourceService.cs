using BlazorApp2Test.Models;

namespace BlazorApp2Test.Components;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

public class ResourceService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;

    public ResourceService(string accessKey, string secretKey, string serviceUrl, string bucketName)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = serviceUrl
        };
        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        _bucketName = bucketName;
    }

    public async Task<FolderItem> GetFolderStructureAsync(string prefix)
    {
        var folder = new FolderItem { Name = prefix };
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = prefix,
            Delimiter = "/"
        };

        var response = await _s3Client.ListObjectsV2Async(request);

        foreach (var commonPrefix in response.CommonPrefixes)
        {
            folder.SubFolders.Add(await GetFolderStructureAsync(commonPrefix));
        }

        foreach (var s3Object in response.S3Objects)
        {
            if (s3Object.Key != prefix)
            {
                folder.Files.Add(new FileItem
                {
                    Name = s3Object.Key.Substring(prefix.Length),
                    Url = GeneratePreSignedUrl(s3Object.Key)
                });
            }
        }

        return folder;
    }

    private string GeneratePreSignedUrl(string key)
    {
        AWSConfigsS3.UseSignatureVersion4 = true;
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(Helper.R2_URL_EXPIRE_TIME),
        };
        return _s3Client.GetPreSignedURL(request);
    }
}
