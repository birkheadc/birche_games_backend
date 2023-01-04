using MongoDB.Bson;

namespace BircheGamesApi.Services;

public interface IFileService
{
  public Task CopyFileAsync(FileType fileType, IFormFile file, ObjectId id);
}