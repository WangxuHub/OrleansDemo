using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace MyOrleansInterface
{
    public interface IGameGrain : IGrainWithStringKey
    {
        Task<IGameObserver> SubscribeForGameUpdates(IGrainObserver obj);
    }
}
