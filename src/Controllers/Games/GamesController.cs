using BircheGamesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BircheGamesApi.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController: ControllerBase
{

  private readonly IWebHostEnvironment webHostEnvironment;
  public GamesController(IWebHostEnvironment webHostEnvironment)
  {
    this.webHostEnvironment = webHostEnvironment;
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

  [HttpGet]
  [Route("{id}")]
  public IActionResult GetGameById([FromRoute] Guid id)
  {
    return Ok(GetTestGame());
  }

  [HttpGet]
  [Route("profiles")]
  public IActionResult GetAllGameProfiles()
  {
    List<GameProfile> profiles = new();
    
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    profiles.Add(GetTestGame().Profile);
    
    return Ok(profiles);
  }

  private Game GetTestGame()
  {
    Guid id = Guid.NewGuid();
    Game game = new Game()
    {
      Id = id,
      DistName = "dejavu",
      ViewportRatio = 1.5f,
      Profile = new()
      {
          Id = id,
          Title = "Dejavu",
          Description = "Dejavu was created in 1 month for Game Devcember 2022. It is the first game I have published. Some sections turned out much more difficult than I intended, and the story evolved into something much darker than I anticipated. All the same, it was fun to create, and I hope you had fun playing it is well.",
          CoverImage = new()
          {
            GameId = id,
            ImageName = "dejavu.png"
          }
      }
    };
    return game;
  }
}