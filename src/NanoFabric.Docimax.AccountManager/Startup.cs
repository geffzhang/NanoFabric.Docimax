using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.AccountManager
{
    public class Startup : StartupBase
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public override void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "AccountManager",
                areaName: "NanoFabric.Docimax.AccountManager",
                template: "AccountManager/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
