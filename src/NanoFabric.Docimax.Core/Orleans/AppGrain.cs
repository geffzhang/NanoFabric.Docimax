using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.Core.Orleans
{
    public abstract class AppGrain : Grain, IAppGrain
    {
        protected readonly ILogger Logger;

        private string _primaryKey;
        public string PrimaryKey => _primaryKey ?? (_primaryKey = this.GetPrimaryKeyAny());

        public string Source { get; }

        protected AppGrain(ILogger logger)
        {
            Source = GetType().GetDemystifiedName();
            Logger = logger;
        }

        public virtual Task Activate() => Task.CompletedTask;

        public override Task OnActivateAsync()
        {
            if (!_primaryKey.IsNullOrEmpty())
                Logger.LogCritical("[{grain}] Grain PrimaryKey was set before activation! Make sure to null PrimaryKey on deactivation!", Source);

            Logger.LogInformation("[{grain}] activated for key: {grainPrimaryKey}", Source, PrimaryKey);
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            Logger.LogInformation("[{grain}] deactivated for key: {grainPrimaryKey}", Source, PrimaryKey);
            return Task.CompletedTask;
        }
    }

    public abstract class AppGrain<TState> : Grain<TState>, IAppGrain
        where TState : new()
    {
        protected readonly ILogger Logger;

        private string _primaryKey;
        public string PrimaryKey => _primaryKey ?? (_primaryKey = this.GetPrimaryKeyAny());

        public string Source { get; }

        protected AppGrain(ILogger logger)
        {
            Source = GetType().GetDemystifiedName();
            Logger = logger;
        }

        public virtual Task Activate() => Task.CompletedTask;

        public override Task OnActivateAsync()
        {
            if (!_primaryKey.IsNullOrEmpty())
                Logger.LogCritical("[{grain}] Grain PrimaryKey was set before activation! Make sure to null PrimaryKey on deactivation!", Source);

            Logger.LogInformation("[{grain}] activated for key: {grainPrimaryKey}", Source, PrimaryKey);
            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            Logger.LogInformation("[{grain}] deactivated for key: {grainPrimaryKey}", Source, PrimaryKey);
            return Task.CompletedTask;
        }

    }
}
