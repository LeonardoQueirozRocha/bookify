using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments.Errors;

public static class ApartmentErrors
{
    public static Error NotFound = new(
        "Apartment.NotFound",
        "The apartment with the specified identifier was not found");
}