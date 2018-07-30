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
                    ClientId = "597c2429f4b27d74",
                    ClientName = "CAS System MPA Client",
                    ClientSecrets = new [] { new Secret("96eb098a127f74bad17badc5ea395b69".Sha256()) },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedScopes = new [] { "DocimaxHerosApi", "DocimaxHeros","AccountTransfer",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile },
                    RedirectUris = { Configuration["Clients:MvcClient:RedirectUri"] },
                    PostLogoutRedirectUris = { Configuration["Clients:MvcClient:PostLogoutRedirectUri"] },
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
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
