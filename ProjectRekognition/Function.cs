using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ProjectRekognition
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> FunctionHandler(String input, ILambdaContext context)
        {
            var rekognitionClient = new AmazonRekognitionClient();

            var array = input.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            string name = (array[0] + "/" + array[0] + ".jpg");

            var response = await rekognitionClient.CompareFacesAsync(new CompareFacesRequest
            {
                SimilarityThreshold = 90,
                SourceImage = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = "s3rekognition",
                        Name = name
                    }
                },

                TargetImage = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = "s3rekognition",
                        Name = array[1]
                    }
                }
            });


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