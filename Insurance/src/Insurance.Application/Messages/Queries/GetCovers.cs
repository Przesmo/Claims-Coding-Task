using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class GetCovers
{
    [DefaultValue(PaginationConstraints.OffsetDefaultValue)]
    [Range(PaginationConstraints.OffsetMinValue, PaginationConstraints.OffsetMaxValue,
        ErrorMessage = "The {0} value needs to be between {1} and {2}")]
    public int Offset { get; set; } = PaginationConstraints.OffsetDefaultValue;

    [DefaultValue(PaginationConstraints.LimitDefaultValue)]
    [Range(PaginationConstraints.LimitMinValue, PaginationConstraints.LimitMaxValue,
        ErrorMessage = "The {0} value needs to be between {1} and {2}")]
    public int Limit { get; set; } = PaginationConstraints.LimitDefaultValue;
}
