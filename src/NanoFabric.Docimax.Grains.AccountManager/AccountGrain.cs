using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Grains.Contracts.AccountManager;
using Orleans;
using Orleans.Transactions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Grains.AccountManager
{
    [Serializable]
    public class Balance
    {
        public uint Value { get; set; } = 1000;
    }

    public class AccountGrain : AppGrain, IAccountGrain
    {
        private readonly ITransactionalState<Balance> balance;

        public AccountGrain(ILogger<AccountGrain> logger, [TransactionalState("balance")] ITransactionalState<Balance> balance)
            :base(logger)
        {
            this.balance = balance ?? throw new ArgumentNullException(nameof(balance));
        }

        Task IAccountGrain.Withdraw(uint amount)
        {
            this.balance.State.Value -= amount;
            this.balance.Save();
            return Task.CompletedTask;
        }

        Task IAccountGrain.Deposit(uint amount)
        {
            this.balance.State.Value += amount;
            this.balance.Save();
            return Task.CompletedTask;
        }

        Task<uint> IAccountGrain.GetBalance()
        {
            return Task.FromResult(this.balance.State.Value);
        }
    }
}
