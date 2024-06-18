using MongoDB.Bson.Serialization.Attributes;

namespace Insurance.Infrastructure.Repositories.Claims;

public class Claim
{
    [BsonId]
    public string Id { get; set; } = null!;

    [BsonElement("coverId")]
    public string CoverId { get; set; } = null!;

    [BsonElement("created")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Created { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("claimType")]
    public ClaimType Type { get; set; }

    [BsonElement("damageCost")]
    public decimal DamageCost { get; set; }
}

public enum ClaimType
{
    Collision = 0,
    Grounding = 1,
    BadWeather = 2,
    Fire = 3
}
