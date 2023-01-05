using BircheGamesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BircheGamesApi.Filters;

public class PasswordAuthAttribute : Attribute, IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    // Refuse access if no password is included in request
    if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var password) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Refuse access if password is included but is wrong
    IPasswordService passwordService = context.HttpContext.RequestServices.GetRequiredService<IPasswordService>();
    if (await passwordService.IsPasswordCorrect(password) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Allow access
    await next();
  }
}