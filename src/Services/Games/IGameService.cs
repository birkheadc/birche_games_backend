using BircheGamesApi.Models;
using MongoDB.Bson;

namespace BircheGamesApi.Services;

public interface IGameService
{
  public Task<List<GameViewModel>> GetGamesAsync();
  public Task<GameViewModel> GetGameAsync(ObjectId id);
  public Task<List<GameProfileViewModel>> GetGameProfilesAsync();
  public Task<GameViewModel?> CreateAsync(NewGameDto newGame);
  public Task DeleteGameById(ObjectId id);
}