using System.IdentityModel.Tokens.Jwt;
using BircheGamesApi.Models;

namespace BircheGamesApi.Services;

public interface ISessionService
{
  public SessionToken GenerateSessionToken();
  public bool ValidateSessionToken(string token);
}