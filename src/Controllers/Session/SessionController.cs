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
  [HttpGet]
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

  [TokenAuth]
  [HttpPost]
  [Route("validate")]
  public IActionResult ValidateSessionToken()
  {
    try
    {
      return Ok();
    }
    catch
    {
      return Forbid();
    }
  }
}