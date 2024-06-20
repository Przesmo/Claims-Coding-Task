namespace Insurance.Application.Messages.Queries;

public static class PaginationConstraints
{
    public const int OffsetMinValue = 0;
    public const int OffsetMaxValue = int.MaxValue;
    public const int OffsetDefaultValue = 0;

    public const int LimitMinValue = 1;
    public const int LimitMaxValue = 5000;
    public const int LimitDefaultValue = 100;
}
