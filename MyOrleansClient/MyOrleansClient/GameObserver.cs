using MyOrleansInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansClient
{
    /// <summary>
    /// Observer class that implements the observer interface. Need to pass a grain reference to an instance of this class to subscribe for updates.
    /// </summary>
    class GameObserver : IGameObserver
    {
        // Receive updates
        public void UpdateGameScore(string score)
        {
            Console.WriteLine("New game score: {0}", score);
        }
    }
}
