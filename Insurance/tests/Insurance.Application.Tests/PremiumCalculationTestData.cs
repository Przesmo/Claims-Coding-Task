using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.Repositories.Covers;
using System.Collections;

namespace Insurance.Application.Tests;

public class PremiumCalculationTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Case: Yacht with first period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(29),
                CoverType = CoverType.Yacht
            },
            39875m
        };

        // Case: Yacht with second period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(40),
                CoverType = CoverType.Yacht
            },
            54312.500m
        };

        // Case: Yacht with third period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(200),
                CoverType = CoverType.Yacht
            },
            288612.500m
        };

        // Case: PassengerShip with first period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(15),
                CoverType = CoverType.PassengerShip
            },
            22500m
        };

        // Case: PassengerShip with second period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(50),
                CoverType = CoverType.PassengerShip
            },
            74400m
        };

        // Case: PassengerShip with third period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(299),
                CoverType = CoverType.PassengerShip
            },
            482745m
        };

        // Case: Tanker with first period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(11),
                CoverType = CoverType.Tanker
            },
            20625m
        };

        // Case: Other with first period
        yield return new object[]
        {
            new ComputePremium
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(25),
                CoverType = CoverType.BulkCarrier
            },
            40625m
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
