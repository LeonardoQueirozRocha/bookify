using Bookify.Domain.Shared.ValuesObjects;

namespace Bookify.Domain.Bookings.ValuesObjects;

public record PricingDetails(
    Money PriceForPeriod,
    Money CleaningFee,
    Money AmenitiesUpCharge,
    Money TotalPrice);