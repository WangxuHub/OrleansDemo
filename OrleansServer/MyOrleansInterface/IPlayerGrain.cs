using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansInterface
{
    //an example of a Grain Interface
    public interface IPlayerGrain : IGrainWithGuidKey
    {
        Task<IGameGrain> GetCurrentGame();
        Task JoinGame(IGameGrain game);
        Task LeaveGame(IGameGrain game);
    }
}
