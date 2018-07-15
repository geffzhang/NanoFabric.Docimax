using NanoFabric.Docimax.Grains.Contracts.Heroes;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Heroes.Client
{
    public class HeroClient : IHeroClient
    {
        private readonly IClusterClient _clusterClient;

        public HeroClient(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public Task<Hero> Get(string key)
        {
            var grain = _clusterClient.GetHeroGrain(key);
            return grain.Get();
        }

        public Task<List<Hero>> GetAll(HeroRoleType? role = null)
        {
            var grain = _clusterClient.GetHeroCollectionGrain();
            return grain.GetAll(role);
        }
    }
}
