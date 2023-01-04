using BircheGamesApi.Models;
using MongoDB.Bson;

namespace BircheGamesApi.Repositories;

public interface IGameRepository
{
  public Task<List<Game>> GetGamesAsync();
  public Task<Game> GetGameAsync(ObjectId id);
  public Task<List<GameProfile>> GetGameProfilesAsync();
  public Task CreateAsync(Game game);
}