
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shop2City.Core.Services.Permissions;
using System.Security.Claims;

namespace Shop2City.Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly int _permissionId;

        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

                if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
                {
                    var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim == null)
                    {
                        context.Result = new RedirectResult("/Login");
                        return;
                    }

                    var userId = int.Parse(userIdClaim.Value);

                    if (!await permissionService.CheckPermissionAsync(_permissionId, userId))
                    {
                        context.Result = new RedirectResult($"/Login?{context.HttpContext.Request.Path}");
                    }
                }
                else
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            catch (Exception ex)
            {
                // Logging (می‌توانید به جای استفاده مستقیم از ILogger، از Middleware استفاده کنید)
                context.Result = new RedirectResult("/Error");
            }
        }
    }

}
