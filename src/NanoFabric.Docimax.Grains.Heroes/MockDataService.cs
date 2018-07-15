using NanoFabric.Docimax.Grains.Contracts.Heroes;
using System.Collections.Generic;
using System.Linq;

namespace NanoFabric.Docimax.Grains.Heroes
{
    public static class MockDataService
    {
        private static readonly List<Hero> MockData = new List<Hero>
        {
            new Hero {Name = "Rengar", Key = "rengar", Role = HeroRoleType.Assassin, Abilities = new HashSet<string> { "savagery", "battle-roar", "bola-strike", "thrill-of-the-hunt"}},
            new Hero {Name = "Kha'zix", Key = "kha-zix", Role = HeroRoleType.Assassin, Abilities = new HashSet<string> { "taste-their-fear", "void-spike", "leap", "void-assault"}},
            new Hero {Name = "Singed", Key = "singed", Role = HeroRoleType.Tank, Abilities = new HashSet<string> { "poison-trail", "mega-adhesive", "fling", "insanity-potion"}}
        };

        public static List<Hero> GetHeroes()
        {
            return MockData;
        }

        public static Hero GetById(string key)
        {
            return MockData.FirstOrDefault(x => x.Key == key);
        }
    }
}