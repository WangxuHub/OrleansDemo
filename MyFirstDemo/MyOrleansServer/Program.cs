using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Orleans服务端";

            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            
            siloConfig.Defaults.DefaultTraceLevel = Orleans.Runtime.Severity.Warning;
            siloConfig.Defaults.TraceToConsole = true;

            var logFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrleansLogs", "{0}-{1}.log");

            var dir = Path.GetDirectoryName(logFile);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            siloConfig.Defaults.TraceFilePattern = logFile;

            var silo = new SiloHost("Test Silo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();
            
            Console.WriteLine("Press Enter to close.");
            // wait here
            Console.ReadLine();

            // shut the silo down after we are done.
            silo.ShutdownOrleansSilo();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
