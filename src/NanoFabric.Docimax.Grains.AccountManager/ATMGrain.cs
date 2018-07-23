using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Grains.Contracts.AccountManager;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.AccountManager
{
    [StatelessWorker]
    public class ATMGrain : AppGrain, IATMGrain
    {
        public ATMGrain(ILogger<ATMGrain> logger) : base(logger)
        {

        }

        Task IATMGrain.Transfer(Guid fromAccount, Guid toAccount, uint amountToTransfer)
        {
            return Task.WhenAll(
                this.GrainFactory.GetGrain<IAccountGrain>(fromAccount).Withdraw(amountToTransfer),
                this.GrainFactory.GetGrain<IAccountGrain>(toAccount).Deposit(amountToTransfer));
        }
    }
}
