using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NanoFabric.Docimax.Identity.Models;
using NanoFabric.Docimax.Identity.Repositories;
using NanoFabric.Docimax.Identity.Services;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NanoFabric.AspNetCore;

namespace NanoFabric.Docimax.Identity
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
            services.AddDbContextPool<IdentityDbContext>(
               options => options.UseSqlServer(Configuration["ConnectionString:Dev"]));
            // IoC - Service & Repository
            services.AddScoped<ILoginUserService, LoginUserService>();
            services.AddScoped<ILoginUserRepository, LoginUserRepository>();
            // IdentityServer4
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            InMemoryConfiguration.Configuration = this.Configuration;
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                    Configuration["Certificates:CerPath"]),
                    Configuration["Certificates:Password"]))
                .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddNanoFabricConsul(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseConsulRegisterService(Configuration);
        }
    }
}
