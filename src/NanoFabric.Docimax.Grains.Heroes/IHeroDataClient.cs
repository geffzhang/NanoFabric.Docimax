using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Heroes
{
    public interface IHeroDataClient
    {
        Task<Hero> GetByKey(string key);
        Task<List<Hero>> GetAll();
    }

    public class MockHeroDataClient : IHeroDataClient
    {
        private readonly ILogger<MockHeroDataClient> _logger;

        public MockHeroDataClient(ILogger<MockHeroDataClient> logger)
        {
            _logger = logger;
        }

        public Task<List<Hero>> GetAll()
        {
            _logger.LogDebug($"[{nameof(GetAll)}] Fetch from mock service");
            return Task.FromResult(MockDataService.GetHeroes().ToList());
        }

        public Task<Hero> GetByKey(string key)
        {
            _logger.LogDebug($"[{nameof(GetByKey)}] Fetching key: {key} from mock service", key);
            return Task.FromResult(MockDataService.GetById(key));
        }
    }
}
