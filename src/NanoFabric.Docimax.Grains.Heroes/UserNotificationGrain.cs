using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Core.Utils;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using Orleans;
using Orleans.Providers;
using SignalR.Orleans.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.Heroes
{
    [StorageProvider(ProviderName = OrleansConstants.GrainPersistenceStorage)]
    public class UserNotificationGrain : AppGrain<UserNotificationState>, IUserNotificationGrain
    {
        private HubContext<IUserNotificationHub> _hubContext;

        public UserNotificationGrain(
            ILogger<HeroGrain> logger
        ) : base(logger)
        {
        }

        public async Task Set(UserNotification item)
        {
            Logger.LogInformation("updating grain state - {item}", item);
            State.UserNotification = item;
            await WriteStateAsync();
        }

        public Task<UserNotification> Get() => Task.FromResult(State.UserNotification);


        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _hubContext = GrainFactory.GetHub<IUserNotificationHub>();

            var item = new UserNotification
            {
                MessageCount = 0
            };
            await Set(item);

            RegisterTimer(async x =>
            {
                State.UserNotification.MessageCount = RandomUtils.GenerateNumber(1, 100);
                await Set(State.UserNotification);

                var userNotification = new UserNotification
                {
                    MessageCount = item.MessageCount
                };

                await _hubContext.User(PrimaryKey).SendSignalRMessage("Broadcast", userNotification);
            }, State, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3));
        }
    }
}
