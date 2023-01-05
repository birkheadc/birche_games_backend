namespace BircheGamesApi.Config;

public class DatabaseConfig
{
  public string ?ConnectionString { get; set; }
  public string ?DatabaseName { get; set; }
  public string ?GamesCollectionName { get; set; }
  public string ?PasswordCollectionName { get; set; }
}