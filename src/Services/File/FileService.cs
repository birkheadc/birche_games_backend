using MongoDB.Bson;

namespace BircheGamesApi.Services;

public class FileService : IFileService
{
  public async Task CopyFileAsync(FileType fileType, IFormFile file, ObjectId id)
  {
    string folderName = fileType == FileType.DIST ? "dists" : "covers";
    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/static", folderName, id.ToString() + (fileType == FileType.DIST ? ".zip" : ".png"));
    Console.WriteLine("Folder to copy to: " + path);

    using (Stream stream = new FileStream(path, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    // Todo: Validate files

    if (fileType == FileType.DIST)
    {
      System.IO.Compression.ZipFile.ExtractToDirectory(path, path.Substring(0, path.Length - 4));
      File.Delete(path);
    }
  }
}