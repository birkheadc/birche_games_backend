using BircheGamesApi.Models;
using BircheGamesApi.Repositories;

namespace BircheGamesApi.Services;

public class GameService : IGameService
{
  private readonly IGameRepository gameRepository;
  private readonly IGameProfileRepository gameProfileRepository;

  public GameService(IGameProfileRepository gameProfileRepository, IGameRepository gameRepository)
  {
    this.gameProfileRepository = gameProfileRepository;
    this.gameRepository = gameRepository;
  }

  public async Task CreateAsync(Game game)
  {
    await gameRepository.CreateAsync(game);
  }

  public async Task<Game> GetGameAsync(Guid id)
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