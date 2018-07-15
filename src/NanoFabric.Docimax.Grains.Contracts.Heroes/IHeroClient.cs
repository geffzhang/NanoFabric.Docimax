using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public interface IHeroClient
    {
        Task<Hero> Get(string key);
        Task<List<Hero>> GetAll(HeroRoleType? role = null);
    }
}
