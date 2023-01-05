namespace BircheGamesApi.Repositories;

public interface IPasswordRepository
{
  /// <summary>
  /// Overwrites the password in the database
  /// </summary>
  public Task ChangeHash(string newPassword);
  /// <summary>
  /// Returns TRUE if the hash matches what is in the database, else FALSE
  /// </summary>
  public Task<bool> ValidateHash(string password);
}