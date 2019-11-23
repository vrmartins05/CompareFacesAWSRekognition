using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Util;

namespace ProjectClient
{
    public class CompareFace
    {
        String accessKey = "";
        String secretKey = "";

        public CompareFace(string accessKey, string secretKey)
        {
            this.accessKey = accessKey;
            this.secretKey = secretKey;
        }

        public async Task<bool> MachFaces(String filePath, String fileName, String personName)
        {
            await UploadToS3(filePath, fileName);

            AmazonLambdaClient alc = new AmazonLambdaClient(accessKey, secretKey, RegionEndpoint.USWest2);
            Amazon.Lambda.Model.InvokeRequest ir = new Amazon.Lambda.Model.InvokeRequest();
            ir.InvocationType = InvocationType.RequestResponse;
            ir.FunctionName = "RekognitionFunction";

            String value = personName + "#" + fileName;

            ir.Payload = "\"" + value + "\"";
            var res = await alc.InvokeAsync(ir);

            var strResponse = Encoding.ASCII.GetString(res.Payload.ToArray());

            if (bool.TryParse(strResponse, out bool retVal))
                return retVal;

            return false;
        }

        public async Task UploadToS3(String filePath, String fileName)
        {
            var client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.USWest2);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = "s3rekognition",
                Key = fileName,
                FilePath = filePath,
                ContentType = "text/plain"
            };

            PutObjectResponse response = await client.PutObjectAsync(putRequest);
        }
    }
}