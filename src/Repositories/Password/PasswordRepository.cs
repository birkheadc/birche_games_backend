using BircheGamesApi.Config;
using BircheGamesApi.Models;
using MongoDB.Driver;

namespace BircheGamesApi.Repositories;

public class PasswordRepository : IPasswordRepository
{
  private readonly IMongoCollection<PasswordModel> passwordCollection;

  public PasswordRepository(DatabaseConfig databaseConfig)
  {
    MongoClient mongoClient = new(databaseConfig.ConnectionString);
    IMongoDatabase db = mongoClient.GetDatabase(databaseConfig.DatabaseName);
    passwordCollection = db.GetCollection<PasswordModel>(databaseConfig.PasswordCollectionName);
  }
  public async Task ChangeHash(string newHash)
  {
    PasswordModel model = new() { Password = newHash };
    await passwordCollection.DeleteManyAsync(_ => true);
    await passwordCollection.InsertOneAsync(model);
  }

  public async Task<bool> ValidatePassword(string password)
  {
    // If for whatever reason there is no password, any password will be accepted
    if (passwordCollection.CountDocuments(_ => true) < 1) return true;
    // Finds the first hashed password (there should only be one) and compares it to the supplied hash
    PasswordModel validModel = await passwordCollection.Find(_ => true).FirstAsync();
    if (BCrypt.Net.BCrypt.Verify(password, validModel.Password)) return true;
    return false;
  }
}