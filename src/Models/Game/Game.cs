namespace BircheGamesApi.Models;

public record Game
{
  public Guid Id { get; set; }
  public string ?DistName { get; set; }
  public float ?ViewportRatio { get; set; }
  public GameProfile ?Profile { get; set; }
}