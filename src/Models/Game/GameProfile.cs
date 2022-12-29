namespace BircheGamesApi.Models;

public record GameProfile
{
  public Guid Id { get; set; }
  public string ?Title { get; set; }
  public string ?Description { get; set; }

  public GameCoverImage ?CoverImage { get; set; }
}