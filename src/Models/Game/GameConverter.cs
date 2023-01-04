namespace BircheGamesApi.Models;

public class GameConverter
{
  public GameViewModel ToViewModel(Game game)
  {
    GameViewModel viewModel = new()
    {
      Id = game.Id.ToString(),
      ViewportRatio = game.ViewportRatio,
      Profile = ToViewModel(game.Profile)
    };
    return viewModel;
  }
  public GameProfileViewModel ToViewModel(GameProfile gameProfile)
  {
    GameProfileViewModel viewModel = new()
    {
      Id = gameProfile.Id.ToString(),
      Title = gameProfile.Title,
      Description = gameProfile.Description
    };
    return viewModel;
  }
}