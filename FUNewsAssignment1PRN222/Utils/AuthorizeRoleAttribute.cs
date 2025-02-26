namespace FUNewsAssignment1PRN222.Utils
{
	using Business.Interfaces;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class AuthorizeRoleAttribute : ActionFilterAttribute
	{
		private readonly string[] _roles;

		public AuthorizeRoleAttribute(params string[] roles)
		{
			_roles = roles;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userRole = context.HttpContext.Session.GetString("UserRole");
			var status = context.HttpContext.Session.GetString("UserStatus");

			if (string.IsNullOrEmpty(userRole) || !_roles.Contains(userRole) || status == "0")
			{
				context.Result = new RedirectToActionResult("Forbidden", "Home", null);
			}
		}
	}

}
