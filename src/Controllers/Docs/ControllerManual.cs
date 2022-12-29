namespace BircheGamesApi.Controllers;

public record ControllerManual
{
  public string ?ControllerName { get; set; }
  public string ?Description { get; set; }
  public string ?Endpoint { get; set; }
  public List<ControllerDoc> ?Docs { get; set; }
}