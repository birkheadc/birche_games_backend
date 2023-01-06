using MongoDB.Bson;

namespace BircheGamesApi.Models;

public record PasswordModel
{
  public ObjectId Id { get; set; }
  public string Password { get; set; } = string.Empty;
}