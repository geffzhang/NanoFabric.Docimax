using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public interface IUserNotificationHub
    {
        Task Broadcast(UserNotification item);
        Task MessageCount(int count);
    }
}
