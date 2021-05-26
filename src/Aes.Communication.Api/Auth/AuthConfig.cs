using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace Aes.Communication.Api.Auth
{
    public class AuthConfig
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
            {
                ClientId = "dev_client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets = { new Secret("dev_secret".Sha256())},

                // scopes that client has access to
                AllowedScopes = { "aes_communications" }
            },
            new Client
            {
                ClientId = "bilateral_client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets = { new Secret("oBnIbE%ghQY6oLCZ$si#d43#cHia@4Y%ZriZSEc6tLLrrlqo1r".Sha256())},

                // scopes that client has access to
                AllowedScopes = { "aes_communications" }
            },
            //PASSWORD
            new Client
            {
                ClientId = "internal_dev",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "aes_communications",
                    "communications_user"
                }
            },

        };

        public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),

            //custom user info
            new IdentityResource(
                name:        "communications_user",
                displayName: "Communications User Info",
                claimTypes:  new[] { "roles" })
        };

        public static IEnumerable<ApiResource> Apis = new List<ApiResource>
        {
            new ApiResource("aes_communications", "AES Communications Api")
            {
                //claims we want passed back in the jwt
                //UserClaims = new [] { "email", "roles" } 
                UserClaims = new [] { "roles" }
            }
        };
    }
}
