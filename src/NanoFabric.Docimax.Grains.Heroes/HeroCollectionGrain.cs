using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Heroes
{
    [StorageProvider(ProviderName = OrleansConstants.GrainMemoryStorage)]
    public class HeroCollectionGrain : AppGrain<HeroCollectionState>, IHeroCollectionGrain
    {
        private readonly IHeroDataClient _heroDataClient;

        public HeroCollectionGrain(
            ILogger<HeroCollectionGrain> logger,
            IHeroDataClient heroDataClient

        ) : base(logger)
        {
            _heroDataClient = heroDataClient;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            if (State.HeroKeys == null)
                await FetchFromRemote();
        }

        public async Task Set(List<Hero> heroes)
        {
            State.HeroKeys = heroes.ToDictionary(x => x.Key, x => x.Role);
            await WriteStateAsync();
        }

        public async Task<List<Hero>> GetAll(HeroRoleType? role = null)
        {
            var query = State.HeroKeys.AsQueryable();

            if (role.HasValue)
                query = query.Where(x => x.Value == role);

            var heroIds = query
                .Select(x => x.Key)
                .ToList();

            var promises = new List<Task<Hero>>();
            foreach (var heroId in heroIds)
            {
                var heroGrain = GrainFactory.GetHeroGrain(heroId);
                promises.Add(heroGrain.Get());
            }

            var result = await Task.WhenAll(promises);
            return result.ToList();
        }

        private async Task FetchFromRemote()
        {
            var heroes = await _heroDataClient.GetAll();
            await Set(heroes);
        }
    }
}