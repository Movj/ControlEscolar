using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CE.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Throttle the thread pool (set availabe threads to amount of processors)
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
