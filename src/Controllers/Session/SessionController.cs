using BircheGamesApi.Filters;
using BircheGamesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheGamesApi.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController: ControllerBase
{

  private readonly ISessionService sessionService;

  public SessionController(ISessionService sessionService)
  {
    this.sessionService = sessionService;
  }

  [PasswordAuth]
  [HttpPost]
  public IActionResult GetSessionToken()
  {
    try
    {
      return Ok(sessionService.GenerateSessionToken());
    }
    catch
    {
      return Forbid();
    }
  }
}