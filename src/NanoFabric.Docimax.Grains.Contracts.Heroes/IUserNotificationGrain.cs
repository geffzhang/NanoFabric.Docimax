using NanoFabric.Docimax.Core.Orleans;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    public interface IUserNotificationGrain : IGrainWithStringKey, IAppGrainContract
    {
        Task Set(UserNotification item);
        Task<UserNotification> Get();
    }
}
