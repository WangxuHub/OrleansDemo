using MyOrleansInterface;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansServer
{
    public class GameGrain : Grain, IGameGrain
    {
        public Task SubscribeForGameUpdates(IGameObserver observer)
        {
            observer.UpdateGameScore(DateTime.Now.ToShortTimeString());
            return Task.CompletedTask;
        }
    }

    public class GameGrain2 : Grain, IGameGrain
    {
        public Task SubscribeForGameUpdates(IGameObserver observer)
        {
            observer.UpdateGameScore("2___" + DateTime.Now.ToShortTimeString());
            return Task.CompletedTask;
        }
    }
}
