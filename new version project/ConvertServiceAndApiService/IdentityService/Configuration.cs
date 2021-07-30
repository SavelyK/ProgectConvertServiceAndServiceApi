using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityService
{
    public class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("RepositoryWebApi", "Web Api")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("RepositoryWebApi", "Web Api", new []
                {JwtClaimTypes.Name })
                {
                    Scopes = { "RepositoryWebApi" }
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "repository-web-api",
                    ClientName = "Repository Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "http://.../signin-oidc"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://..."
                    },
                    PostLogoutRedirectUris =
                    {
                        "http:/.../signout-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "RepositoryWebApi"
                    },
                    AllowAccessTokensViaBrowser = true


                }
            };
    }
}
