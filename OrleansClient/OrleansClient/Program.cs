using MyOrleansInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Core;
using Orleans.Runtime.Configuration;
using Orleans.Configuration;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace OrleansClient
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.Title = "Orleans客户端";
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await StartClientWithRetries())
                {
                    await DoClientWork(client);

                    while (true)
                    {
                        try
                        {
                            await DoPersistenceWork(client);
                        }
                        catch(Exception ee)
                        {
                            Console.WriteLine(ee.GetBaseException().Message);
                        }
                        Console.ReadKey();
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e);
                
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "HelloWorldApp";
                        })
                       // .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHello).Assembly).WithReferences())
                       // .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PlayerGrain).Assembly).WithReferences())
                        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning).AddConsole())
                        .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Good morning, my friend!");

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n{0}\n\n", response);
            Console.ForegroundColor = oldColor;
        }

        private static Task DoPersistenceWork(IClusterClient client)
        {
            var clientList = new List<IMyPersistenceGrain>();
            Enumerable.Range(0, 10).ToList().ForEach(a=> {
                var grain = client.GetGrain<IMyPersistenceGrain>(a);
                var curValue = grain.DoRead().Result;
                if (string.IsNullOrWhiteSpace(curValue))
                {
                    grain.DoWrite($"{a}__{DateTime.Now.ToShortTimeString()}").Wait();
                }
                clientList.Add(grain);
            });

            clientList.ForEach(item=> {
                var curValue = item.DoRead().Result;
                Console.WriteLine($"grainID {item.GetPrimaryKeyLong()}  , value {curValue}");
            });
            return Task.CompletedTask;
        }
    }
}
