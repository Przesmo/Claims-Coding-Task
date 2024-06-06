namespace Insurance.Host.Messages.Queries;

// ToDo: Add validation
public class GetCovers
{
    public int Offset { get; set; }
    public int Limit { get; set; }
}
