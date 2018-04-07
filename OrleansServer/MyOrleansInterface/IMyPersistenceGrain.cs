using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrleansInterface
{
    public interface IMyPersistenceGrain : Orleans.IGrainWithIntegerKey
    {
        Task DoWrite(string val);

        Task<string> DoRead();
    }
}
