using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BircheGamesApi.Config;
using BircheGamesApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BircheGamesApi.Services;

public class SessionService : ISessionService
{
  private readonly JwtConfig jwtConfig;

  public SessionService(JwtConfig jwtConfig)
  {
    this.jwtConfig = jwtConfig;
  }
  public SessionToken GenerateSessionToken()
  {
    Console.WriteLine("Here's your token!");
    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    SecurityTokenDescriptor descriptor = new()
    {
      Expires = DateTime.UtcNow.AddDays(1),
      Issuer = jwtConfig.Issuer,
      Audience = jwtConfig.Audience,
      SigningCredentials = new
      (
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key ?? "")),
        SecurityAlgorithms.HmacSha512Signature
      )
    };
    SecurityToken token = tokenHandler.CreateToken(descriptor);
    Console.WriteLine(tokenHandler.WriteToken(token));
    return new SessionToken()
    {
      Token = tokenHandler.WriteToken(token)
    };
  }
}