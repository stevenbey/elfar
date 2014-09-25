namespace Elfar.Web.Routing
{
    public class DeveloperConstraint : RolesConstraint
    {
        protected override string[] Roles
        {
            get { return roles; }
        }

        static readonly string[] roles = { "Dev", "Developer", "Development" };
    }
}