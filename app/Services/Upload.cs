using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace app.Services
{
  public class Upload
  {
    private IAmazonS3 S3 { get; }
    private PutObjectResponse response { get; set; }

    public Upload(IAmazonS3 s3) => this.S3 = s3;

    public async Task<PutObjectResponse> Run(string filePath, string key)
    {
      string bucket = "educat-images";
      try
      {
        var putRequest = new PutObjectRequest
        {
          BucketName = bucket,
          Key = key,
          FilePath = filePath,
        };
        response = await S3.PutObjectAsync(putRequest);
      }
      catch (AmazonS3Exception e)
      {
        Console.WriteLine(
                "Error encountered ***. Message:'{0}' when writing an object"
                , e.Message);
        Console.WriteLine($"Params bucket = {bucket} filePath = {filePath} key = {key}");
      }
      catch (Exception e)
      {
        Console.WriteLine(
            "Unknown encountered on server. Message:'{0}' when writing an object"
            , e.Message);
        Console.WriteLine($"Params bucket = {bucket} filePath = {filePath} key = {key}");
      }
      return response;
    }
  }
}
