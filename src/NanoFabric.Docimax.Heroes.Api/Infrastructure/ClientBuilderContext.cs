﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core;
using Orleans;
using System;

namespace NanoFabric.Docimax.Heroes.Api.Infrastructure
{
    public class ClientBuilderContext
    {
        public string ClusterId => AppInfo.ClusterId;
        public string ServiceId => AppInfo.Name;
        public string ConsulEndPoint => AppInfo.ConsulEndPoint;
        public ILogger Logger { get; set; }
        public IAppInfo AppInfo { get; set; }
        public IConfiguration Configuration { get; set; }
        public Action<IClientBuilder> ConfigureClientBuilder { get; set; }
    }
}
