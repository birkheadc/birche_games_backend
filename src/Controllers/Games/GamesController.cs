using BircheGamesApi.Models;
using BircheGamesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheGamesApi.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController: ControllerBase
{

  private readonly IWebHostEnvironment webHostEnvironment;
  private readonly IGameService gameService;
  public GamesController(IWebHostEnvironment webHostEnvironment, IGameService gameService)
  {
    this.webHostEnvironment = webHostEnvironment;
    this.gameService = gameService;
  }

  [HttpGet]
  [Route("help")]
  public IActionResult GetHelp()
  {
    ControllerManual manual = new()
    {
      ControllerName = "Games Controller",
      Description = "Perform CRUD operations on Games database.",
      Endpoint = "/api/games",
      Docs = new List<ControllerDoc>()
      {
        new ControllerDoc()
        {
          Endpoint = "/help",
          Description = "Display this manual.",
          Returns = "Object of type ControllerManual."
        },
        new ControllerDoc()
        {
          Endpoint = "/{id}",
          Description = "Get the Game model of a specific ID",
          Returns = "Object of type Game or NotFound error."
        },
        new ControllerDoc()
        {
          Endpoint = "/profiles",
          Description = "Get a list of profiles of every game.",
          Returns = "List of objects of type GameProfile, possibly an empty list."
        }
      }
    };
    return Ok(manual);
  }

  // [HttpPost]
  // public async Task<IActionResult> PostGame([FromForm] FileModel file)
  // {
  //   try
  //   {
  //     Console.WriteLine("Hello there");
  //     string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TESTFILENAME");
  //     using (Stream stream = new FileStream(path, FileMode.Create))
  //     {
  //       await file.File.CopyToAsync(stream);
  //     }
  //     return Ok();
  //   }
  //   catch
  //   {
  //     return StatusCode(9001);
  //   }
  // }

  [HttpPost]
  public async Task<IActionResult> PostGame([FromForm] NewGameDto newGame)
  {
    try
    {
      Guid id = Guid.NewGuid();
      Game game = new()
      {
        Id = id,
        ViewportRatio = newGame.ViewportRatio,
        Profile = new()
        {
          Id = id,
          Title = newGame.Title,
          Description = newGame.Description
        }
      };
      Console.WriteLine("Dist length = " + newGame.Dist.Length);
      await gameService.CreateAsync(game);
      return Ok(game);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetGameById([FromRoute] Guid id)
  {
    Console.WriteLine("Looking for game of id: " + id);
    try
    {
      Game game = await gameService.GetGameAsync(id);
      if (game == null)
      {
        return NotFound();
      }
      return Ok(game);
    }
    catch
    {
      return BadRequest();
    }
  }

  [HttpGet]
  [Route("profiles")]
  public async Task<IActionResult> GetAllGameProfiles()
  {
    List<GameProfile> profiles = await gameService.GetGameProfilesAsync();
  
    return Ok(profiles);
  }
}