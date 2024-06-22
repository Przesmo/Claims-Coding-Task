using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.Services;

public static class PremiumCalculator
{
    /*
        Depending on how often discounts/periods (maybe other factor like client type) changes.
        We can think about improving function configuration. Like reading paremeters from some file
    */
    private const int FirstPeriodDays = 30;
    private const int SecondPeriodDays = 180;
    private const int ThirdPeriodDays = 365;
    private const decimal YachtDiscountSecondPeriod = 0.05m;
    private const decimal YachtDiscountThirdPeriod = 0.08m;
    private const decimal OtherDiscountSecondPeriod = 0.02m;
    private const decimal OtherDiscountThirdPeriod = 0.03m;

    public static decimal Calculate(ComputePremium query)
    {
        var insuranceLength = (query.EndDate - query.StartDate).Days;

        var firstPeriod = Math.Min(insuranceLength, FirstPeriodDays);
        var secondPeriod = Math.Min(Math.Max(insuranceLength - firstPeriod, 0), SecondPeriodDays);
        var thirdPeriod = Math.Min(Math.Max(insuranceLength - SecondPeriodDays, 0), ThirdPeriodDays);

        var premiumPerDay = GetPremiumPerDay(query.CoverType);
        var totalPremium = firstPeriod * premiumPerDay;

        if (query.CoverType == CoverType.Yacht)
        {
            totalPremium += GetPremium(premiumPerDay, secondPeriod, YachtDiscountSecondPeriod);
            totalPremium += GetPremium(premiumPerDay, thirdPeriod, YachtDiscountThirdPeriod);
        }
        else
        {
            totalPremium += GetPremium(premiumPerDay, secondPeriod, OtherDiscountSecondPeriod);
            totalPremium += GetPremium(premiumPerDay, thirdPeriod, OtherDiscountThirdPeriod);
        }

        return totalPremium;
    }

    private static decimal GetPremium(decimal premiumPerDay, int periodDays, decimal discount) =>
        periodDays * premiumPerDay * (1 - discount);

    private static decimal GetPremiumPerDay(CoverType coverType)
    {
        var multiplier = coverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };

        return 1250 * multiplier;
    }
}
