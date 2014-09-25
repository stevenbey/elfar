namespace Elfar.Web.Routing
{
    public class AdminConstraint : RolesConstraint
    {
        protected override string[] Roles
        {
            get { return roles; }
        }

        static readonly string[] roles = { "Admin", "Administrator", "Administration" };
    }
}