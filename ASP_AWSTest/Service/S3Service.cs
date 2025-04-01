using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ASP_AWSTest.Constant;
using ASP_AWSTest.IService;
using ASP_AWSTest.Response;

namespace ASP_AWSTest.Service;

public class S3Service(IAmazonS3 amazonS3Client) : IS3Service
{
    private readonly IAmazonS3 _amazonS3Client = amazonS3Client;

    public async Task<(Response<PutBucketResponse>, HttpStatusCode)> CreateS3BucketAsync(string bucketName)
    {
        try
        {
            var result = await _amazonS3Client.PutBucketAsync(bucketName);
            return (Response<PutBucketResponse>.SuccessResult(result, "Bucket created sucessfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<PutBucketResponse>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }

    public async Task<(Response<ListObjectsResponse>, HttpStatusCode)> ListFilesInBucketAsync(string bucketName)
    {
        try
        {
            var result = await _amazonS3Client.ListObjectsAsync(bucketName);

            return (Response<ListObjectsResponse>.SuccessResult(result), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<ListObjectsResponse>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }

    public async Task<(Response<List<string>>, HttpStatusCode)> ListFileNamesAsync(string bucketName)
    {
        try
        {
            var result = await _amazonS3Client.ListObjectsAsync(bucketName);

            var fileNames = result.S3Objects.Select(obj => obj.Key).ToList();

            return (Response<List<string>>.SuccessResult(fileNames), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<List<string>>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
    
    public async Task<(Response<string>, HttpStatusCode)> UploadFileAsync(IFormFile file, string bucketName)
    {
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = file.OpenReadStream(),
                Key = file.FileName,
                BucketName = bucketName,
                CannedACL = S3CannedACL.NoACL
            };
            using (var transferUtility = new TransferUtility(_amazonS3Client))
            await transferUtility.UploadAsync(uploadRequest);

            return (Response<string>.SuccessResult(data:$"BucketName: {bucketName}, FileName: {file.FileName}", $"Uploaded file {file.FileName} to bucket {bucketName} successfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<string>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
    public async Task<(Response<string>, HttpStatusCode)> UploadFolderAsync(string bucketName, string directoryPath)
    {
        try
        {
            var uploadDirectoryRequest = new TransferUtilityUploadDirectoryRequest
            {
                BucketName = bucketName,
                Directory = directoryPath,
                SearchPattern = "*",
                SearchOption = SearchOption.AllDirectories,
            };
            using (var transferUtility = new TransferUtility(_amazonS3Client))
                await transferUtility.UploadDirectoryAsync(uploadDirectoryRequest);

            return (Response<string>.SuccessResult(data: $"BucketName: {bucketName}", $"Uploaded folder to bucket {bucketName} successfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<string>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }

    public async Task<(Response<string>, HttpStatusCode)> DownloadFileToLocalAsync(string bucketName, string objectKey)
    {
        try
        {
            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                FilePath = S3Const.downloadFilePath
            };
            using (var transferUtility = new TransferUtility(_amazonS3Client)) // Error: Access denioed to local folder
                await transferUtility.DownloadAsync(downloadRequest);
            return (Response<string>.SuccessResult(data:"Download Successfull", $"File '{objectKey}' from bucket '{bucketName}' downloaded successfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<string>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
    public async Task<(byte[], string)> GetDownloadFileAsync(string bucketName, string objectKey)
    {
        try
        {
            var downloadRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey,
            };
            var response = await _amazonS3Client.GetObjectAsync(downloadRequest);
            var contentType = response.Headers.ContentType;
            byte[] file;
            using (var stream = new MemoryStream())
            {
                response.ResponseStream.CopyTo(stream);
                file = stream.ToArray();
            };
            return (file, contentType);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<(Response<string>, HttpStatusCode)> CreatePresignedUrlAsync(string bucketName, string fileName)
    {
        try
        {
            var getPresignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.Now.AddDays(1),
                Verb = HttpVerb.GET
            };
            var presignedUrl = await _amazonS3Client.GetPreSignedURLAsync(getPresignedUrlRequest);
            return (Response<string>.SuccessResult(presignedUrl, message:"Genereated url successfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<string>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
    public async Task<(Response<DeleteObjectResponse>, HttpStatusCode)> DeleteFileInBucketAsync(string bucketName, string objectKey)
    {
        try
        {
            var result = await _amazonS3Client.DeleteObjectAsync(bucketName, objectKey);
            return (Response<DeleteObjectResponse>.SuccessResult(result, $"Deleted file {objectKey} from bucket {bucketName}."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<DeleteObjectResponse>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
    public async Task<(Response<string>, HttpStatusCode)> DeleteFolderInBucketAsync(string bucketName, string directory)
    {
        try
        {
            if (!directory.EndsWith('/'))
            {
                directory += "/";
            }
            var listObjectRequest = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = directory
            };

            var response = await _amazonS3Client.ListObjectsV2Async(listObjectRequest);
            var keys = response.S3Objects.Select(obj => obj.Key).ToList();

            if (keys.Count > 0)
            {
                foreach (var key in keys)
                {
                    await _amazonS3Client.DeleteObjectAsync(bucketName, key);
                }
                return (Response<string>.SuccessResult($"Directory {directory} deleted successfully.", $"Deleted directory {directory} from bucket {bucketName}."), HttpStatusCode.OK);
            }
            return (Response<string>.FailureResult($"No directory {directory} found."), HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return (Response<string>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }

    public async Task<(Response<DeleteBucketResponse>, HttpStatusCode)> DeleteBucketAsync(string bucketName)
    {
        try
        {
            // List all files in bucket
            var listObjects = await _amazonS3Client.ListObjectsAsync(bucketName);
            var keys = listObjects.S3Objects.Select(obj => obj.Key).ToList();

            // delete files if present inside bucket
            foreach (var key in keys)
            {
                await _amazonS3Client.DeleteObjectAsync(bucketName, key);
            }

            // delete bucket 
            var result = await _amazonS3Client.DeleteBucketAsync(bucketName);
            return (Response<DeleteBucketResponse>.SuccessResult(result, $"Bucket: {bucketName} deleted successfully."), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return (Response<DeleteBucketResponse>.FailureResult(ex.Message), HttpStatusCode.BadRequest);
        }
    }
}
