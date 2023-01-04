using BircheGamesApi.Models;
using BircheGamesApi.Repositories;
using MongoDB.Bson;

namespace BircheGamesApi.Services;

public class GameService : IGameService
{
  private readonly IGameRepository gameRepository;
  private readonly IGameProfileRepository gameProfileRepository;
  private readonly IFileService fileService;

  public GameService(IGameProfileRepository gameProfileRepository, IGameRepository gameRepository, IFileService fileService)
  {
    this.gameProfileRepository = gameProfileRepository;
    this.gameRepository = gameRepository;
    this.fileService = fileService;
  }

  public async Task<Game?> CreateAsync(NewGameDto newGame)
  {
    if (IsGameValid(newGame) == false) return null;
    ObjectId id = ObjectId.GenerateNewId();
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
    await fileService.CopyFileAsync(FileType.DIST, newGame.Dist!, id);
    await fileService.CopyFileAsync(FileType.COVERIMAGE, newGame.CoverImage!, id);
    await gameRepository.CreateAsync(game);
    return game;
  }

  private bool IsGameValid(NewGameDto newGame)
  {
    if (newGame.Title.Length < 1) return false;
    if (newGame.Description.Length < 1) return false;
    if (newGame.ViewportRatio <= 0.0f) return false;
    if (newGame.Dist is null) return false;
    if (newGame.CoverImage is null) return false;

    return true;
  }

  public async Task<Game> GetGameAsync(ObjectId id)
  {
    Game game = await gameRepository.GetGameAsync(id);
    return game;
  }

  public async Task<List<GameProfile>> GetGameProfilesAsync()
  {
    return await gameRepository.GetGameProfilesAsync();
  }

  public async Task<List<Game>> GetGamesAsync()
  {
    List<Game> games = await gameRepository.GetGamesAsync();
    return games;
  }
}