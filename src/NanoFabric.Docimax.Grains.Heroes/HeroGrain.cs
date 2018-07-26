using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Core.Utils;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using Orleans.Providers;
using Orleans.Streams;
using SignalR.Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Heroes
{
    [StorageProvider(ProviderName = OrleansConstants.GrainPersistenceStorage)]
    public class HeroGrain : AppGrain<HeroState>, IHeroGrain
    {
        private readonly IHeroDataClient _heroDataClient;

        public HeroGrain(
            ILogger<HeroGrain> logger,
            IHeroDataClient heroDataClient
        ) : base(logger)
        {
            _heroDataClient = heroDataClient;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            if (State.Hero == null)
            {
                var hero = await _heroDataClient.GetByKey(PrimaryKey);
                await Set(hero);
            }

            var streamProvider = GetStreamProvider(Constants.STREAM_PROVIDER);
            var stream = streamProvider.GetStream<Hero>(StreamConstants.HeroStream, $"hero:{PrimaryKey}");

            RegisterTimer(async x =>
            {
                State.Hero.Health = RandomUtils.GenerateNumber(1, 100);

                await Task.WhenAll(
                    Set(State.Hero),
                    stream.OnNextAsync(State.Hero)
                );

            }, State, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3));
        }

        public Task Set(Hero hero)
        {
            State.Hero = hero;
            return WriteStateAsync();
        }

        public Task<Hero> Get() => Task.FromResult(State.Hero);

    }
}
