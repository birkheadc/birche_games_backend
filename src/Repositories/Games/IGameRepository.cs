using BircheGamesApi.Models;

namespace BircheGamesApi.Repositories;

public interface IGameRepository
{
  public Task<List<Game>> GetGamesAsync();
  public Task<Game> GetGameAsync(Guid id);
  public Task<List<GameProfile>> GetGameProfilesAsync();
  public Task CreateAsync(Game game);
}