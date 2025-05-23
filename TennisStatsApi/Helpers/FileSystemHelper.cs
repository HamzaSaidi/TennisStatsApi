namespace TennisStatsApi.Helpers;

public class FileSystemHelper:IFileSystemHelper
{
    
    public string ReadAllText(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}