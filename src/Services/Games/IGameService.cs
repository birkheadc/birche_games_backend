using BircheGamesApi.Models;
using MongoDB.Bson;

namespace BircheGamesApi.Services;

public interface IGameService
{
  public Task<List<Game>> GetGamesAsync();
  public Task<Game> GetGameAsync(ObjectId id);
  public Task<List<GameProfile>> GetGameProfilesAsync();
  public Task<Game?> CreateAsync(NewGameDto newGame);
}