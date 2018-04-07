using MyOrleansInterface;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansServer
{
    [StorageProvider(ProviderName = "GrainStorageForTest")]
    public class MyPersistenceGrain : Grain<MyGrainState>, IMyPersistenceGrain
    {
        public Task DoWrite(string val)
        {
            State.Field1 = val;
            return base.WriteStateAsync();
        }

        public async Task<string> DoRead()
        {
            await base.ReadStateAsync();
            return State.Field1;
        }

    }

}
