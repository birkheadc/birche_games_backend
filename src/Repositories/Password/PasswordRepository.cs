namespace BircheGamesApi.Repositories;

public class PasswordRepository : IPasswordRepository
{
  public Task ChangeHash(string newPassword)
  {
    throw new NotImplementedException();
  }

  public Task<bool> ValidateHash(string password)
  {
    throw new NotImplementedException();
  }
}