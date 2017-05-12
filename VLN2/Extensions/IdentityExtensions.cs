using Microsoft.Owin.Security;
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
        public static string GetDescription(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Description");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }
    }
}