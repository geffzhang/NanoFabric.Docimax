using System;
using System.Threading.Tasks;
using Xunit;

namespace NanoFabric.Docimax.Tests
{
    public class HerosTests : BaseAPITest
    {
        [Fact]
        public async Task GetHeroTest()
        {
            var response = await _client.GetAsync("/api/heroes");
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
