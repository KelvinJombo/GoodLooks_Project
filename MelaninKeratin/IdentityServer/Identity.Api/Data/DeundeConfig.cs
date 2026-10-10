
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.Api.Data;

public static class DuendeConfig
{
    public static IEnumerable<IdentityResource> GetIdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> GetApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog.api", "Catalog API"),
            new ApiScope("orders.api", "Orders API")
        };

    public static IEnumerable<Client> GetClients =>
        new Client[]
        {
            new Client
            {
                ClientId = "melaninkeration.mobile",
                ClientName = "MelaninKeration MAUI App",

                // Native public client: Authorization Code + PKCE
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                // Replace with the actual MAUI callback URI.
                RedirectUris =
                {
                    "melaninkeration://callback"
                },

                PostLogoutRedirectUris =
                {
                    "melaninkeration://logout"
                },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "catalog.api",
                    "orders.api"
                },

                AllowOfflineAccess = true
            }
        };
}