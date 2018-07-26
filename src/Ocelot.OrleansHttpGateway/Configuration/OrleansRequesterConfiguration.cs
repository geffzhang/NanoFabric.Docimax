﻿using Ocelot.Middleware;
using Ocelot.OrleansHttpGateway.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.OrleansHttpGateway.Configuration
{
    public class OrleansRequesterConfiguration
    {
        public OrleansRequesterConfiguration()
        {
            MapRouteToGraininterface = (route) => route.GrainName;
        }
        public Action<DownstreamContext> RequestContextInjection { get; set; }

        public Func<GrainRouteValues, string> MapRouteToGraininterface { get; set; }
    }
}
