using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3ByteStreamPOC
{
    class Program
    {
        private const string bucketName = "Null";
        private const string keyName = "Null";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static string CreatingFilePath = @"E:\Shamshad_Workspace\copiedSongs.mp3";
        private static string awsAccessKeyId = "Null";
        private static string awsSecretAccessKeyId = "Null";

        private static IAmazonS3 client;
        static void Main(string[] args)
        {
            client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKeyId,bucketRegion);
            ReadObjectData().Wait();
        }
        static async Task ReadObjectData()
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };  
                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                {
                    BinaryReader br = new BinaryReader(responseStream);
                    FileStream fileStream = new FileStream(CreatingFilePath, FileMode.Create);

                    

                    while (true)
                    {

                        byte[] b = br.ReadBytes(1000);

                        Console.WriteLine(b.Length);

                        if (b.Length == 0)
                            break;

                        for (int i = 0; i < b.Length; i++)
                        {
                            fileStream.WriteByte(b[i]);
                        }
                    }
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e);
                Console.ReadLine();
            }
        }
    }
}
