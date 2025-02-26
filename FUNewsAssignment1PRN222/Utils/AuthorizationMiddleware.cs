using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Services
{
	public class AuthorizationMiddleware
	{
		private readonly RequestDelegate _next;

		public AuthorizationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var user = context.User;
			if (user.Identity != null && user.Identity.IsAuthenticated)
			{
				var role = user.FindFirst(ClaimTypes.Role)?.Value;

				if (!HasAccess(context, role))
				{
					context.Response.StatusCode = StatusCodes.Status403Forbidden;
					await context.Response.WriteAsync("Access Denied");
					return;
				}
			}

			await _next(context);
		}

		private bool HasAccess(HttpContext context, string? role)
		{
			var path = context.Request.Path.Value?.ToLower();

			if (string.IsNullOrEmpty(role))
			{
				if (path.StartsWith("/home") || (path.StartsWith("/newsarticle") && context.Request.Query["status"] == "active"))
				{
					return true;
				}
				return false;
			}

			switch (role)
			{
				case "2":
					if (path.StartsWith("/NewsArticle") && context.Request.Query["status"] == "active")
					{
						return true;
					}
					break;

				case "3":
					if (path.StartsWith("/User"))
					{
						return true;
					}
					if (path.StartsWith("/report"))
					{
						return true;
					}
					break;

				case "1":
					if (path.StartsWith("/Category") || path.StartsWith("/NewsArticle") || path.StartsWith("/Profile"))
					{
						return true;
					}
					var userId = context.Session.GetString("UserId");
					var requestedUserId = context.Request.Query["userId"];
					if (path.StartsWith("/news/history") && userId == requestedUserId)
					{
						return true;
					}
					break;
			}

			return false;
		}

	}
}
