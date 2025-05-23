namespace TennisStatsApi.Helpers;

public interface IFileSystemHelper
{
    public string ReadAllText(string filePath);

    public bool FileExists(string filePath);
}