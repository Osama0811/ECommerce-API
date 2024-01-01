using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CircuitsUc.Application.Helpers.CommenEnum;

namespace CircuitsUc.Api.ActionFilter
{
    public class BaseActionFilter : IAsyncActionFilter
    {

        private readonly ISecurityUserService _SecurityUserService;
        public BaseActionFilter(ISecurityUserService SecurityUserService)
        {
            _SecurityUserService = SecurityUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            string UserName = context.HttpContext.GetUserId();


            if (string.IsNullOrEmpty(UserName))
            {
                context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary(new { controller = "Message", action = "LoginFirst" }));
            }
            else
            {
                Guid userId = Guid.Parse(context.HttpContext.GetUserId());
                var User = _SecurityUserService.GetActiveUserById(userId);
                if (User == null)
                {
                    context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Message", action = "UserNotFound" }));
                }

                else if (User.RoleId != (long)RoleType.Admin)
                {
                    context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Message", action = "NoPermission" }));
                }
                else
                {
                    await next();
                }
            }

        }



    }
}