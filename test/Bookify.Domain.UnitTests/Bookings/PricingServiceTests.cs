using Bookify.Domain.Bookings.Services;
using Bookify.Domain.Bookings.ValuesObjects;
using Bookify.Domain.Shared.ValuesObjects;
using Bookify.Domain.UnitTests.Apartments;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class PricingServiceTests
{
    private const string ClassName = nameof(PricingService);
    private const string Method = nameof(PricingService.CalculatePrice);

    private readonly PricingService _service = new();

    [Fact(DisplayName = $"{ClassName} {Method} should return correct total price")]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var period = DateRange.Create(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 31));

        var expectedTotalPrice = new Money(
            price.Amount * period.LenghtInDays,
            price.Currency);

        var apartment = ApartmentData.Create(price);

        // Act
        var pricingDetails = _service.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }

    [Fact(DisplayName = $"{ClassName} {Method} should return correct total price when cleaning fee is included")]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        var price = new Money(10.0m, Currency.Usd);
        var cleaningFee = new Money(99.99m, Currency.Usd);
        var period = DateRange.Create(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 31));

        var expectedTotalPrice = new Money(
            price.Amount * period.LenghtInDays + cleaningFee.Amount,
            price.Currency);

        var apartment = ApartmentData.Create(price, cleaningFee);

        // Act
        var pricingDetails = _service.CalculatePrice(apartment, period);

        // Assert
        pricingDetails.TotalPrice.Should().Be(expectedTotalPrice);
    }
}