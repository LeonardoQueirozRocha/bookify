namespace Bookify.Domain.Apartments.ValuesObjects;

public record Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street);