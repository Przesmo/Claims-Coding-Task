using System.Text.RegularExpressions;

namespace Insurance.ComponentTests.Configuration.Options;

internal class MongoDbOptions
{
    public string Username { get; } = null!;
    public string Password { get; } = null!;
    public int Port { get; }

    public MongoDbOptions(string connectionString)
    {
        var pattern = @"mongodb://(.*?):(.*?)@.*?:(\d+)$";
        var match = Regex.Match(connectionString, pattern);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid connection string");
        }

        Username = match.Groups[1].Value;
        Password = match.Groups[2].Value;
        Port = int.Parse(match.Groups[3].Value);
    }
}
