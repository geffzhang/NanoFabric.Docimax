using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public interface IHeroHub
    {
        Task Send(string message);
        Task Broadcast(Hero hero);
        Task StreamUnsubscribe(string methodName, string id);
    }
}
