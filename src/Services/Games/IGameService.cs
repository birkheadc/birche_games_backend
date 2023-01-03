using BircheGamesApi.Models;

namespace BircheGamesApi.Services;

public interface IGameService
{
  public Task<List<Game>> GetGamesAsync();
  public Task<Game> GetGameAsync(Guid id);
  public Task<List<GameProfile>> GetGameProfilesAsync();
  public Task CreateAsync(Game game);
}