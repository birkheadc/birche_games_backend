namespace BircheGamesApi.Repositories;

public interface IPasswordRepository
{
  /// <summary>
  /// Overwrites the password in the database.
  /// </summary>
  public Task ChangeHash(string newHash);
  /// <summary>
  /// Returns <c>true</c> if the hash matches what is in the database, else false.
  /// </summary>
  public Task<bool> ValidatePassword(string password);
}