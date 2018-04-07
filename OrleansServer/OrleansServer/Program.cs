using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrleansServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.Title = "Orleans服务端";
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var tick = new System.Diagnostics.Stopwatch();
                tick.Start();
                var host = await StartSilo();
                tick.Stop();
                Console.WriteLine($"Use {tick.Elapsed.Milliseconds}ms，Press Enter to terminate...----------------------");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            string invariant = "System.Data.SqlClient";
            string dbConnectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OrleansServerDb;Integrated Security=True";

            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => {
                    logging.SetMinimumLevel(LogLevel.Warning);
                    logging.AddConsole();
                })
                //集群
                .UseAdoNetClustering(options =>
                {
                    options.ConnectionString = dbConnectString;
                    options.Invariant = invariant;
                })
                //use AdoNet for reminder service
                .UseAdoNetReminderService(options =>
                {
                    options.Invariant = invariant;
                    options.ConnectionString = dbConnectString;
                })
                //use AdoNet for Persistence
                .AddAdoNetGrainStorage("GrainStorageForTest", options =>
                {
                    options.Invariant = invariant;
                    options.ConnectionString = dbConnectString;
                }); ;

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
