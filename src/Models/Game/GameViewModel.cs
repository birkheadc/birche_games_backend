namespace BircheGamesApi.Models;

public record GameViewModel
{
  public string Id { get; set; } = string.Empty;
  public float ViewportRatio { get; set; } = 1.0f;
  public GameProfileViewModel Profile { get; set; } = new GameProfileViewModel();
}