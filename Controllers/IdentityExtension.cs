using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MessingSystem.Controllers
{
    public static class IdentityExtension
    {
        public static string GetId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

            Claim claim = claimsIdentity.FindFirst(ClaimTypes.Sid);

            return claim.Value;
        }
    }
}
