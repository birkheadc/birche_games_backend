namespace BircheGamesApi.Models;

public record GameCoverImage
{
  public Guid GameId { get; set; }
  public string ?ImageName { get; set; }
}