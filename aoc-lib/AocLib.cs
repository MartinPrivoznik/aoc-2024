using aoc_lib.Models;
using aoc_lib.Utils;

namespace aoc_lib;

public class AocLib(string sessionKey)
{
    public async Task<InputData<T>> DownloadInputData<T>(int year, int day)
    {
        if(TryGetLocalData(year, day, out var localData))
            return new InputData<T>(localData);
        
        var endpoint = BuildDataEndpoint(year, day);
        var data = await AocHttpClient.GetAsync(endpoint, sessionKey);
        await SaveInputData(year, day, data);
        
        return new InputData<T>(data);
    }

    private static string BuildDataEndpoint(int year, int day)
    {
        return $"/{year}/day/{day}/input";
    }
    
    private static bool TryGetLocalData(int year, int day, out string data)
    {
        var path = $"./data/{year}/day{day}.txt";

        if (!File.Exists(path))
        {
            data = string.Empty;
            return false;
        }

        data = File.ReadAllText(path);
        return true;
    }

    private static async Task SaveInputData(int year, int day, string data)
    {
        var path = $"./data/{year}/day{day}.txt";
        var directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            if (directory != null) Directory.CreateDirectory(directory);
        }
        
        await File.WriteAllTextAsync(path, data);
    }
}