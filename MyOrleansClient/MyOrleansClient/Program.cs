using MyOrleansInterface;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Orleans客户端";

            RunWatcher().Wait();

            Console.WriteLine("客户端启动成功");

            Console.ReadLine();

            while (true)
            {
                Console.ReadLine();
            }
        }

        static async Task RunWatcher()
        {
            try

            {
                // Connect to local silo
                var config = ClientConfiguration.LocalhostSilo();

                config.DefaultTraceLevel = Orleans.Runtime.Severity.Warning;

                var clientBuilder = new ClientBuilder().UseConfiguration(config);
               
                var client = clientBuilder.Build();
                await client.Connect();

                // Hardcoded player ID
                string playerId = "双反光";
                IPlayerGrain player = client.GetGrain<IPlayerGrain>(playerId);
                IGameGrain game = null;

                while (game == null)
                {
                    Console.WriteLine("Getting current game for player {0}...", playerId);

                    try
                    {
                        game = await player.GetCurrentGame();
                        if (game == null) // Wait until the player joins a game
                        {
                            //await Task.Delay(5000);

                            var newGame = client.GetGrain<IGameGrain>("王者荣耀");
                            await player.JoinGame(newGame);
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Exception: ", exc.GetBaseException());
                    }
                }

                Console.WriteLine("Subscribing to updates for game {0}...", game.GetPrimaryKeyString());

                // Subscribe for updates
                var watcher = new GameObserver();
                await game.SubscribeForGameUpdates(
                    await client.CreateObjectReference<IGameObserver>(watcher));

                Console.WriteLine("Subscribed successfully. Press <Enter> to stop.");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Unexpected Error: {0}", exc.GetBaseException());
            }
        }
    }
}
