using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aes.Communication.Application;
using IdentityModel;

namespace Aes.Communication.Api
{
    public class ClaimsService
    {
        public static AppUser CreateAppUser(IEnumerable<Claim> claims)
        {
            var claimList = claims.ToList();
            return new AppUser
            {
                UserId = Convert.ToInt32(claimList.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value),
                Roles = claimList.FirstOrDefault(c => c.Type == "roles")?.Value.Split(",").Select(r => Convert.ToInt32(r)).ToArray(),
                OrganizationId = Convert.ToInt32(claimList.FirstOrDefault(c => c.Type == "org_id")?.Value)
            };
        }
    }
}
