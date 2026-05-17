using Bookify.Domain.Apartments.Entities;
using Bookify.Domain.Apartments.Enums;
using Bookify.Domain.Apartments.ValuesObjects;
using Bookify.Domain.Shared.ValuesObjects;

namespace Bookify.Application.UnitTests.Apartments;

internal static class ApartmentData
{
    public static Apartment Create()
    {
        var id = Guid.NewGuid();
        var name = new Name("Test apartment");
        var description = new Description("Test description");
        var address = new Address(
            "Country",
            "State",
            "ZipCode",
            "City",
            "Street");
        var price = new Money(100.0m, Currency.Usd);
        var cleaningFee = Money.Zero();
        var amenities = new List<Amenity>();

        return new Apartment(
            id,
            name,
            description,
            address,
            price,
            cleaningFee,
            amenities);
    }
}