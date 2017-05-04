using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace VLN2.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetDisplayname(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Displayname");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}