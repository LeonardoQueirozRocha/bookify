using Bookify.Domain.Apartments.Entities;
using Bookify.Domain.Apartments.Enums;
using Bookify.Domain.Apartments.ValuesObjects;
using Bookify.Domain.Shared.ValuesObjects;

namespace Bookify.Domain.UnitTests.Apartments;

internal static class ApartmentData
{
    public static Apartment Create(
        Money price,
        Money? cleaningFee = null)
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

        cleaningFee ??= Money.Zero();
        List<Amenity> amenities = [];

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