using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NanoFabric.Docimax.Grains.Contracts.Heroes;

namespace NanoFabric.Docimax.Heroes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroClient _client;

        public HeroesController(IHeroClient client)
        {
            _client = client;
        }

        // GET api/heroes
        [HttpGet]
        public async Task<List<Hero>> Get()
        {
            var result = await _client.GetAll().ConfigureAwait(false);
            return result;
        }

        // GET api/heroes/5
        [HttpGet("{id}")]
        public Task<Hero> Get(string id)
        {
            return _client.Get(id);
        }
    }
}