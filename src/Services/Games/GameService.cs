using BircheGamesApi.Models;
using BircheGamesApi.Repositories;
using MongoDB.Bson;

namespace BircheGamesApi.Services;

public class GameService : IGameService
{
  private readonly IGameRepository gameRepository;
  private readonly IFileService fileService;
  private readonly GameConverter converter;

  public GameService(IGameRepository gameRepository, IFileService fileService)
  {
    this.gameRepository = gameRepository;
    this.fileService = fileService;
    converter = new GameConverter();
  }

  public async Task<GameViewModel?> CreateAsync(NewGameDto newGame)
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
    await fileService.CopyFileAsync(FileType.ZIP, newGame.Dist!, id);
    await fileService.CopyFileAsync(FileType.COVERIMAGE, newGame.CoverImage!, id);
    await gameRepository.CreateAsync(game);
    return converter.ToViewModel(game);
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

  public async Task<GameViewModel> GetGameAsync(ObjectId id)
  {
    Game game = await gameRepository.GetGameAsync(id);
    GameViewModel viewModel = converter.ToViewModel(game);
    return viewModel;
  }

  public async Task<List<GameProfileViewModel>> GetGameProfilesAsync()
  {
    List<GameProfile> profiles = await gameRepository.GetGameProfilesAsync();
    List<GameProfileViewModel> viewModels = new();
    foreach (GameProfile profile in profiles)
    {
      viewModels.Add(converter.ToViewModel(profile));
    }
    return viewModels;
  }

  public async Task<List<GameViewModel>> GetGamesAsync()
  {
    List<Game> games = await gameRepository.GetGamesAsync();
    List<GameViewModel> viewModels = new();
    foreach (Game game in games)
    {
      viewModels.Add(converter.ToViewModel(game));
    }
    return viewModels;
  }

  public async Task DeleteGameById(ObjectId id)
  {
    bool wasDeleted = await gameRepository.DeleteGameById(id);
    if (wasDeleted == true)
    {
      await DeleteGameFiles(id);
    }
  }

  private async Task DeleteGameFiles(ObjectId id)
  {
    await fileService.DeleteFileAsync(FileType.FOLDER, id);
    await fileService.DeleteFileAsync(FileType.COVERIMAGE, id);
  }
}