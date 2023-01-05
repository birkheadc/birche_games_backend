using BircheGamesApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BircheGamesApi.Controllers;

[ApiController]
[Route("api/password")]
public class PasswordController: ControllerBase
{
  [PasswordAuth]
  public IActionResult ValidatePassword()
  {
    return Ok();
  }
}