namespace BircheGamesApi.Services;

public interface IPasswordService
{
  public Task<bool> ChangePassword(string newPassword);
  public Task<bool> IsPasswordCorrect(string password);
}