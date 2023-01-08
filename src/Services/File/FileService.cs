using MongoDB.Bson;

namespace BircheGamesApi.Services;

public class FileService : IFileService
{
  public async Task CopyFileAsync(FileType fileType, IFormFile file, ObjectId id)
  {
    string path = GetPath(fileType, id);
    using (Stream stream = new FileStream(path, FileMode.Create))
    {
      await file.CopyToAsync(stream);
    }

    // Todo: Validate files

    if (fileType == FileType.ZIP)
    {
      System.IO.Compression.ZipFile.ExtractToDirectory(path, path.Substring(0, path.Length - 4));
      File.Delete(path);
    }
  }

  public async Task DeleteFileAsync(FileType fileType, ObjectId id)
  {
    string path = GetPath(fileType, id);
    try
    {
      if (fileType == FileType.COVERIMAGE || fileType == FileType.ZIP)
      {
        File.Delete(path);
      } 
      if (fileType == FileType.FOLDER)
      {
        DirectoryInfo directory = new(path);
        directory.Delete(true);
      }
    }
    catch
    {
      // Todo: Log this
      // File was probably not found
    }
    
  }

  private string GetPath(FileType fileType, ObjectId id)
  {
    string folderName = fileType == FileType.COVERIMAGE ? "covers" : "dists";
    string extension = "";
    if (fileType == FileType.ZIP)
    {
      extension = ".zip";
    }
    if (fileType == FileType.COVERIMAGE)
    {
      extension = ".png";
    }
    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/static", folderName, id.ToString() + extension);
    return path;
  }
}