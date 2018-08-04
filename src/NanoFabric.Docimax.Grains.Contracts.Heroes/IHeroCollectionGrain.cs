//using NanoFabric.Docimax.Core.Orleans;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public interface IHeroCollectionGrain : IGrainWithIntegerKey //, IAppGrainContract
    {
        Task Set(List<Hero> heroes);
        Task<List<Hero>> GetAll(HeroRoleType? role = null);
    }

    public class HeroCollectionState
    {
        public Dictionary<string, HeroRoleType> HeroKeys { get; set; }
    }
}
