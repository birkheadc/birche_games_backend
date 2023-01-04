namespace BircheGamesApi.Models;

public record GameProfileViewModel
{
  public string Id { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}