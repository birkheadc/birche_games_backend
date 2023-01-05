using BircheGamesApi.Repositories;

namespace BircheGamesApi.Services;

public class PasswordService : IPasswordService
{

  private readonly IPasswordRepository passwordRepository;

  public PasswordService(IPasswordRepository passwordRepository)
  {
    this.passwordRepository = passwordRepository;
  }

  public async Task<bool> ChangePassword(string newPassword)
  {
    if (IsPasswordValid(newPassword) == false)
    {
      return false;
    }
    string hash = PasswordToHash(newPassword);
    await passwordRepository.ChangeHash(hash);
    return true;
  }

  public async Task<bool> IsPasswordCorrect(string password)
  {
    return await passwordRepository.ValidatePassword(password);
  }

  private bool IsPasswordValid(string password)
  {
    // Assume valid, add restrictions later if necessary
    return true;
  }

  private string PasswordToHash(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }
}