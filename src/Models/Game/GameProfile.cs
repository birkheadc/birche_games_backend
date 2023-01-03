namespace BircheGamesApi.Models;

public record GameProfile
{
  public Guid Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}