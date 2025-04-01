using ASP_AWSTest.IService;
using Microsoft.AspNetCore.Mvc;

namespace ASP_AWSTest.Controllers;

[Route("s3")]
[ApiController]
public class S3Controller : ControllerBase
{
    private readonly IS3Service _s3Service;

    public S3Controller(IS3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost("create/bucket")]
    public async Task<ActionResult> CreateS3BucketAsync(string bucketName)
    {
        var (response, statusCode) = await _s3Service.CreateS3BucketAsync(bucketName);
        return StatusCode((int)statusCode, response);
    }

    [HttpGet("list/files")]
    public async Task<ActionResult> GetAllFilesInS3BucketAsync(string bucketName)
    {
        var (response, statusCode) = await _s3Service.ListFilesInBucketAsync(bucketName);
        return StatusCode((int)statusCode, response);
    }

    [HttpGet("list/fileNames")]
    public async Task<ActionResult> GetFileNamesFromBucketAsync(string bucketName)
    {
        var (response, statusCode) = await _s3Service.ListFileNamesAsync(bucketName);
        return StatusCode((int)statusCode, response);
    }

    [HttpPost("upload/file")]
    public async Task<ActionResult> UplaodFileAsync(string bucketName, IFormFile file)
    {
        var (response, statusCode) = await _s3Service.UploadFileAsync(file, bucketName);
        return StatusCode((int)statusCode, response);
    }

    [HttpPost("upload/folder")]
    public async Task<ActionResult> UplaodFolderAsync(string bucketName, string directoryPath)
    {
        var (response, statusCode) = await _s3Service.UploadFolderAsync(bucketName, directoryPath);
        return StatusCode((int)statusCode, response);
    }

    [HttpGet("download/file-to-local")]
    public async Task<ActionResult> DownloadFileToLocalFolderAsync(string bucketName, string fileName)
    {
        var (response, statusCode) = await _s3Service.DownloadFileToLocalAsync(bucketName, fileName);
        return StatusCode((int)statusCode, response);
    }

    [HttpGet("download/filelink")]
    public async Task<ActionResult> GetDownloadFileAsync(string bucketName, string fileName)
    {
        var (file, contentType) = await _s3Service.GetDownloadFileAsync(bucketName, fileName);
        return File(file, contentType, fileName);
    }

    [HttpGet("presigned-url")]
    public async Task<ActionResult> GetPreSignedUrlAsync(string bucketName, string fileName)
    {
        var (response, statusCode) = await _s3Service.CreatePresignedUrlAsync(bucketName, fileName);
        return StatusCode((int)statusCode, response);
    }

    [HttpDelete("delete/file")]
    public async Task<ActionResult> DeleteFileAsync(string bucketName, string fileName)
    {
        var (response, statusCode) = await _s3Service.DeleteFileInBucketAsync(bucketName, fileName);
        return StatusCode((int)statusCode, response);
    }

    [HttpDelete("delete/folder")]
    public async Task<ActionResult> DeleteFolderAsync(string bucketName, string directoryName)
    {
        var (response, statusCode) = await _s3Service.DeleteFolderInBucketAsync(bucketName, directoryName);
        return StatusCode((int)statusCode, response);
    }

    [HttpDelete("delete/bucket")]
    public async Task<ActionResult> DeleteBucketAsync(string bucketName)
    {
        var (response, statusCode) = await _s3Service.DeleteBucketAsync(bucketName);
        return StatusCode((int)statusCode, response);
    }
}
