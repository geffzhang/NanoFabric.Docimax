using Microsoft.AspNetCore.SignalR;
using NanoFabric.Docimax.Grains.Contracts.Heroes;

namespace NanoFabric.Docimax.Heroes.Api.Realtime
{
    public class UserNotificationHub : Hub<IUserNotificationHub>
    {

    }
}
