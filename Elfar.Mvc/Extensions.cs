using System.Linq;
using System.Security.Principal;

namespace Elfar.Mvc
{
    static class Extensions
    {
        public static bool IsInRoles(this IPrincipal principal, params string[] roles)
        {
            return roles.Any(principal.IsInRole);
        }
    }
}