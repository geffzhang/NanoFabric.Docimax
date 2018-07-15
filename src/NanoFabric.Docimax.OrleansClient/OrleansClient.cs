using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoFabric.Docimax.OrleansClient
{
    public class OrleansClient : IOrleansClient
    {
        private readonly Dictionary<string, IClusterClient> clients = new Dictionary<string, IClusterClient>();
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        public OrleansClient(IServiceProvider serviceProvider, ILogger<OrleansClient> logger)
        {
            this.Logger = logger;
            this.ServiceProvider = serviceProvider;
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            return i;

        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            return i;

        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            return i;

        }


        /// <summary>
        /// 获取Orleans ClusterClient
        /// </summary>
        /// <typeparam name="TGrainInterface"></typeparam>
        private IClusterClient GetClusterClient<TGrainInterface>()
        {
            IClusterClient client = null;
            string name = typeof(TGrainInterface).Namespace;

            int attempt = 0;
            while (true)
            {
                try
                {
                    if (clients.ContainsKey(name))
                        client = clients[name];
                    else
                    {
                        lock (clients)
                        {
                            if (!clients.ContainsKey(name))
                            {
                                client = BuilderClient(name);
                                clients.Add(name, client);
                            }
                            else
                                client = clients[name];
                        }
                    }

                    if (!client.IsInitialized)
                    {
                        client.Connect().Wait();
                        Logger.LogDebug($"Connection {name} Sucess...");
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                    attempt++;
                    Logger.LogError($"Connection {name} Faile...");
                    throw new Exception($"Connection {name} Faile...");
                }
                return client;
            }
        }

        private IClusterClient BuilderClient(string name)
        {
            IClientBuilder builder = this.ServiceProvider.GetRequiredServiceByName<IClientBuilder>(name);
            return builder.Build();
        }
      
    }
}
