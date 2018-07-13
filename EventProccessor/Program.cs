using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EventProccessor
{
    class Program
    {
        static async System.Threading.Tasks.Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("json1.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var host = new EventProcessorHost(
                configuration
            );
            


        }
    }
}
