namespace BircheGamesApi.Models;

public record NewGameDto
{
  public string Title { get; set; } = "";
  public string Description { get; set; } = "";
  public float ViewportRation { get; set; } = 1.0f;
  public IFormFile? Dist { get; set; }
  public IFormFile? CoverImage { get; set; }
}