using System.Text.RegularExpressions;

namespace Auditing.ComponentTests.Configuration.Options;

internal class MsSQLOptions
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public MsSQLOptions(string connectionString)
    {
        var pattern = @"User Id=(.*?);Password=(.*?);";
        var match = Regex.Match(connectionString, pattern);
        if (!match.Success)
        {
            throw new ArgumentException("Invalid connection string");
        }

        Username = match.Groups[1].Value;
        Password = match.Groups[2].Value;
    }
}
