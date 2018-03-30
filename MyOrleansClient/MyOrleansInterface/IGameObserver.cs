using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansInterface
{
    public interface IGameObserver: IGrainObserver
    {
        void UpdateGameScore(string score);
    }
}
