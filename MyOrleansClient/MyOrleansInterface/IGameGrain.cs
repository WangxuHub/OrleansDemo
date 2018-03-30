using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansInterface
{
    public interface IGameGrain : IGrainWithStringKey
    {
        Task SubscribeForGameUpdates(IGameObserver observer);
    }
}
