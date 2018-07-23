using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using NanoFabric.Docimax.Heroes.Api.Infrastructure;
using NanoFabric.Docimax.Heroes.Api.Realtime;
using NanoFabric.Docimax.Heroes.Client;
using Ocelot.JwtAuthorize;
using Orleans;

namespace NanoFabric.Docimax.Heroes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var appInfo = new AppInfo(Configuration);
            services.AddSingleton<IAppInfo>(appInfo);

            var clientBuilderContext = new ClientBuilderContext
            {
                Configuration = Configuration,
                AppInfo = appInfo,
                ConfigureClientBuilder = clientbuilder =>
                    clientbuilder.ConfigureApplicationParts(x => x.AddApplicationPart(typeof(IHeroCollectionGrain).Assembly).WithReferences())
                    .UseSignalR()
            };      

            services.AddNanoFabricConsul(Configuration);
            services.UseOrleansClient(clientBuilderContext);
            services.AddHeroesClients();
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
            services.AddSignalR().AddOrleans();

            services.AddApiJwtAuthorize((context) =>
            {
                return ValidatePermission(context);
            });
        }

        /// <summary>
        /// Cusomer Validate Method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool ValidatePermission(HttpContext httpContext)
        {
            var permissions = new List<Permission>() {
                new Permission { Name="admin", Predicate="Get", Url="/api/values" },
                new Permission { Name="admin", Predicate="Post", Url="/api/values" }
            };
            var questUrl = httpContext.Request.Path.Value.ToLower();

            if (permissions != null && permissions.Where(w => w.Url.Contains("}") ? questUrl.Contains(w.Url.Split('{')[0]) : w.Url.ToLower() == questUrl && w.Predicate.ToLower() == httpContext.Request.Method.ToLower()).Count() > 0)
            {
                var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;
                var roleArr = roles.Split(',');
                var perCount = permissions.Where(w => roleArr.Contains(w.Name)).Count();
                if (perCount == 0)
                {
                    httpContext.Response.Headers.Add("error", "no permission");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostingEnvironment env, ILoggerFactory loggerFactory	)
		{
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseCors("TempCorsPolicy");

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSignalR(routes =>
                {
                    routes.MapHub<HeroHub>("/realtime/hero");
                    routes.MapHub<UserNotificationHub>("/userNotifications");
                });

                app.UseMvc()
                .UseConsulRegisterService(Configuration);
        }
    }
}
