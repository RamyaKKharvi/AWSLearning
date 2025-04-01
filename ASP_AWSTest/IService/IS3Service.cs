using System.Net;
using Amazon.S3.Model;
using ASP_AWSTest.Response;

namespace ASP_AWSTest.IService;

public interface IS3Service
{
    Task<(Response<string>, HttpStatusCode)> CreatePresignedUrlAsync(string bucketName, string fileName);
    Task<(Response<PutBucketResponse>, HttpStatusCode)> CreateS3BucketAsync(string bucketName);
    Task<(Response<DeleteBucketResponse>, HttpStatusCode)> DeleteBucketAsync(string bucketName);
    Task<(Response<DeleteObjectResponse>, HttpStatusCode)> DeleteFileInBucketAsync(string bucketName, string objectKey);
    Task<(Response<string>, HttpStatusCode)> DeleteFolderInBucketAsync(string bucketName, string directory);
    Task<(Response<string>, HttpStatusCode)> DownloadFileToLocalAsync(string bucketName, string objectKey);
    Task<(byte[], string)> GetDownloadFileAsync(string bucketName, string objectKey);
    Task<(Response<List<string>>, HttpStatusCode)> ListFileNamesAsync(string bucketName);
    Task<(Response<ListObjectsResponse>, HttpStatusCode)> ListFilesInBucketAsync(string bucketName);
    Task<(Response<string>, HttpStatusCode)> UploadFileAsync(IFormFile file, string bucketName);
    Task<(Response<string>, HttpStatusCode)> UploadFolderAsync(string bucketName, string directoryPath);
}
