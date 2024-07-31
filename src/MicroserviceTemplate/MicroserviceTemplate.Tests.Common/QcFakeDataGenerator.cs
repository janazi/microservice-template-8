using Bogus;
using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Tests.Common;

public class FakeDataGenerator
{
    private static readonly string[] VehicleConditions = ["New", "Used", "CPO"];

    /// <summary>
    /// When set generateDataToSeed to false, the Id will be set to 0 to allow insert operations, otherwise it will be auto-incremented to simulate a seeded database.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="generateDataToSeed">If true primary key will not be filled to allow db insertion using EF</param>
    /// <returns></returns>
    public static IEnumerable<Vehicle> GenerateDealerVehicleListings(int amount, bool generateDataToSeed = false)
    {
        Randomizer.Seed = new Random(8675309);
        var dealerVehicleListingId = 0;
        var testVehicles = new Faker<Vehicle>()
            .StrictMode(false)
            .RuleFor(o => o.Id, f => generateDataToSeed ? 0 : ++dealerVehicleListingId)
            .RuleFor(o => o.ProviderId, f => f.Random.Number(1, 2))
            .RuleFor(o => o.IsDeleted, f => false)
            .RuleFor(o => o.UId, f => Guid.NewGuid())
            .RuleFor(o => o.Vin, f => f.Vehicle.Vin())
            .RuleFor(o => o.Make, f => f.Vehicle.Manufacturer())
            .RuleFor(o => o.Model, f => f.Vehicle.Model())
            .RuleFor(o => o.Condition, f => f.PickRandom(VehicleConditions))
            .RuleFor(o => o.DealerName, f => f.Company.CompanyName())
            .RuleFor(o => o.Year, f => f.Date.Past(f.Random.Number(-4, -1)).Year);

        return testVehicles.Generate(amount);
    }

    public static IEnumerable<Image> GenerateImages(int amount, Vehicle vehicle)
    {
        Randomizer.Seed = new Random(8675309);
        var testImages = new Faker<Image>()
            .StrictMode(true)
            .RuleFor(o => o.VehicleId, f => vehicle.Id)
            .RuleFor(o => o.Url, f => f.Image.PicsumUrl())
            .RuleFor(o => o.ImageTypeId, f => f.Random.Number(1, 2));

        return testImages.Generate(amount);
    }

}