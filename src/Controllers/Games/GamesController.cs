using BircheGamesApi.Filters;
using BircheGamesApi.Models;
using BircheGamesApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

  [TokenAuth]
  [HttpPost]
  public async Task<IActionResult> PostGame([FromForm] NewGameDto newGame)
  {
    try
    {
      GameViewModel? game = await gameService.CreateAsync(newGame);
      return Ok(game);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<IActionResult> GetGameById([FromRoute] string id)
  {
    Console.WriteLine("Looking for game of id: " + id);
    try
    {
      GameViewModel game = await gameService.GetGameAsync(ObjectId.Parse(id));
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

  [TokenAuth]
  [HttpDelete]
  [Route("{id}")]
  public async Task<IActionResult> DeleteGameById([FromRoute] string id)
  {
    try
    {
      await gameService.DeleteGameById(ObjectId.Parse(id));
      return Ok();
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
    List<GameProfileViewModel> profiles = await gameService.GetGameProfilesAsync();
  
    return Ok(profiles);
  }
}