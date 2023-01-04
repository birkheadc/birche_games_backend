using MongoDB.Bson;

namespace BircheGamesApi.Models;

public record GameProfile
{
  public ObjectId Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}