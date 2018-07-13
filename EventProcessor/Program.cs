using Microsoft.Extensions.Configuration;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Sandboxable.Microsoft.WindowsAzure.Storage;
using Sandboxable.Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace EventProcessor
{
    class Program
    {
        static async Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("json1.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var connectionString = configuration["StorageConnectionString"];

            //

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var eventStoreContainer = blobClient.GetContainerReference("eventstore");

            var blobs = await eventStoreContainer.ListBlobsSegmentedAsync(
                null, true, BlobListingDetails.All, 100, null, null, null);
            foreach (var blob in blobs.Results)
            {
                if (blob is CloudBlockBlob)
                {
                    var blockBlob = (CloudBlockBlob)blob;

                    using (var stream = await blockBlob.OpenReadAsync())
                    {
                        //using (var container = AvroContainer.CreateGenericReader(stream))
                        //{
                        //    while (container.MoveNext())
                        //    {
                        //        foreach (AvroRecord record in container.Current.Objects)
                        //        {
                        //            var properties = record.GetField<Dictionary<string, object>>("Properties");
                        //            var body = record.GetField<byte[]>("Body");
                        //            var json = Encoding.UTF8.GetString(body);

                        //        }

                        //    }
                        //}
                    }
                }
            }
        }
    }
}
