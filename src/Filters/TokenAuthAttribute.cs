using BircheGamesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BircheGamesApi.Filters;

public class TokenAuthAttribute : Attribute, IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    // Refuse access if no token is included in request
    if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    Console.WriteLine("Authorization header value: " + token);

    // TODO
    // Refuse access if token is included but is invalid
    ISessionService sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
    if (sessionService.ValidateSessionToken(token) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Allow access
    await next();
  }
}