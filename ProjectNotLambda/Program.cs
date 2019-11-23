using System;
using System.IO;
using System.Threading.Tasks;

using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace ProjectNotLambda
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        public async Task<bool> FunctionHandler(String input)
        {
            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            Amazon.Rekognition.Model.Image imageTarget = new Amazon.Rekognition.Model.Image();
            try
            {
                using (FileStream fs = new FileStream(input, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    imageTarget.Bytes = new MemoryStream(data);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load source image: " + input);
                return true;
            }


            //CompareFacesRequest compareFacesRequest = new CompareFacesRequest()
            var response = await rekognitionClient.CompareFacesAsync(new CompareFacesRequest
            {
                SimilarityThreshold = 90,
                SourceImage = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = "s3rekognition",
                        Name = "vi.jpg"
                    }
                },

                TargetImage = imageTarget
            });


            // Call operation
            // CompareFacesResponse compareFacesResponse = rekognitionClient.CompareFacesAsync(response);


            foreach (CompareFacesMatch mach in response.FaceMatches)
            {
                if (mach.Similarity > 90)
                    return true;
                return false;


            }
            return false;
        }
    }
}
