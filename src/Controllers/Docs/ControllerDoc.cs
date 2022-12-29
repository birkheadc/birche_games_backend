namespace BircheGamesApi.Controllers;

public record ControllerDoc
{
  public string ?Endpoint { get; set; }
  public string ?Description { get; set; }
  public string ?Returns { get; set; }
}