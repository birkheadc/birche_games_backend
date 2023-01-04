using BircheGamesApi.Config;
using BircheGamesApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BircheGamesApi.Repositories;

public class GameRepository : IGameRepository
{
  private readonly IMongoCollection<Game> gamesCollection;

  public GameRepository(DatabaseConfig databaseConfig)
  {
    MongoClient mongoClient = new(databaseConfig.ConnectionString);
    IMongoDatabase db = mongoClient.GetDatabase(databaseConfig.DatabaseName);
    gamesCollection = db.GetCollection<Game>(databaseConfig.GamesCollectionName);
  }

  public async Task CreateAsync(Game game)
  {
    Console.WriteLine("Creating game with id: " + game.Id);
    await gamesCollection.InsertOneAsync(game);
  }

  public async Task<Game> GetGameAsync(ObjectId id)
  {
    // TODO: Get game by Id
    Game game = await gamesCollection.Find(_ => true).FirstOrDefaultAsync();
    return game;
  }

  public async Task<List<GameProfile>> GetGameProfilesAsync()
  {
    var gameProfiles = await gamesCollection.Find(_ => true).Project(g => g.Profile).ToListAsync();
    return gameProfiles;
  }

  public async Task<List<Game>> GetGamesAsync()
  {
    List<Game> games = await gamesCollection.Find(_ => true).ToListAsync();
    return games;
  }
}