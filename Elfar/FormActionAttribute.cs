using System.Reflection;
using System.Web.Mvc;

namespace Elfar
{
    class FormActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string name, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.Form[Prefix + methodInfo.Name] != null
                && !controllerContext.IsChildAction;
        }

        public string Prefix = "Action::";
    }
}