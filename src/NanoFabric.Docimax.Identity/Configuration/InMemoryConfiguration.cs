using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace NanoFabric.Docimax.Identity
{
    /// <summary>
    /// One In-Memory Configuration for IdentityServer => Just for Demo Use
    /// </summary>
    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Define which APIs will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("DocimaxHerosApi", "HerosApi Service"),
                new ApiResource("DocimaxHeros", "Heros Service"),
                new ApiResource("AccountTransfer", "AccountTransferApp Service")
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "cas.web.nb",
                    ClientName = "CAS System MPA Client",
                    ClientSecrets = new [] { new Secret("websecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new [] { "clientservice", "productservice",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                },
                new Client
                {
                    ClientId = "cas.mobile.nb",
                    ClientName = "CAS System Mobile App Client",
                    ClientSecrets = new [] { new Secret("mobilesecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new [] { "productservice",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                },
                new Client
                {
                    ClientId = "cas.spa.nb",
                    ClientName = "CAS System SPA Client",
                    ClientSecrets = new [] { new Secret("spasecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new [] { "agentservice", "clientservice", "productservice",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                },
                new Client
                {
                    ClientId = "cas.mvc.nb.implicit",
                    ClientName = "CAS System MVC App Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { Configuration["Clients:MvcClient:RedirectUri"] },
                    PostLogoutRedirectUris = { Configuration["Clients:MvcClient:PostLogoutRedirectUri"] },
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "agentservice", "clientservice", "productservice"
                    },
                    //AccessTokenLifetime = 3600, // one hour
                    AllowAccessTokensViaBrowser = true // can return access_token to this client
                }
            };
        }

        /// <summary>
        /// Define which IdentityResources will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
