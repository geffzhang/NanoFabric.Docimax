using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public static class GrainFactoryExtensions
    {
        public static IHeroGrain GetHeroGrain(this IGrainFactory factory, string key)
            => factory.GetGrain<IHeroGrain>(key);

        public static IHeroCollectionGrain GetHeroCollectionGrain(this IGrainFactory factory)
            => factory.GetGrain<IHeroCollectionGrain>(0);

    }
}
