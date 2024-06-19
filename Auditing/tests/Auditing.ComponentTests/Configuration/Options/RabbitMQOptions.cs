using System.Text.RegularExpressions;

namespace Auditing.ComponentTests.Configuration.Options;

internal class RabbitMQOptions
{
    public string Host { get; } = null!;
    public string Username { get; } = null!;
    public string Password { get; } = null!;

    public RabbitMQOptions(string connectionString)
    {
        var pattern = @"host=(.*?);username=(.*?);password=(.*?)$";
        var match = Regex.Match(connectionString, pattern);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid connection string");
        }

        Host = match.Groups[1].Value;
        Username = match.Groups[2].Value;
        Password = match.Groups[3].Value;
    }
}
