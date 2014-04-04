using System.Linq;
using System.Security.Principal;

namespace Elfar.Mvc
{
    public static class Extensions
    {
        internal static bool IsInRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
}