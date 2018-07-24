using NanoFabric.Docimax.Grains.Contracts.Heroes;
using NanoFabric.Docimax.OrleansClient;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Heroes.Client
{
    public class HeroClient : IHeroClient
    {
        private readonly IOrleansClient _orleansClient;
        private readonly string serviceId;

        public HeroClient(IOrleansClient clusterClient)
        {
            _orleansClient = clusterClient;
            serviceId = "Docimax.Heroes";
        }

        public Task<Hero> Get(string key)
        {
            var grain = _orleansClient.GetGrain<IHeroGrain>(key, serviceId);
            return grain.Get();
        }

        public Task<List<Hero>> GetAll(HeroRoleType? role = null)
        {
            var grain = _orleansClient.GetGrain<IHeroCollectionGrain>(0, serviceId);
            return grain.GetAll(role);
        }
    }
}
