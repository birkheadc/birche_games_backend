namespace BircheGamesApi.Models;

public record Game
{
  public Guid Id { get; set; }
  public float ViewportRatio { get; set; } = 1.0f;
  public GameProfile Profile { get; set; } = new GameProfile();
}