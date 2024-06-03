namespace Claims.Auditing
{
    public class ClaimAudit
    {
        public int Id { get; set; }

        public string? ClaimId { get; set; }

        public DateTime Created { get; set; }

        public string? HttpRequestType { get; set; }

        //To Do: think about some transaction/correlation id to store as well
    }
}
