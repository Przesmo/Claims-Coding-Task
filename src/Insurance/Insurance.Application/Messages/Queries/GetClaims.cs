namespace Insurance.Host.Messages.Queries;

// ToDo: Add validation
public class GetClaims
{
    public int Offset { get; set; }
    public int Limit { get; set; }
}
