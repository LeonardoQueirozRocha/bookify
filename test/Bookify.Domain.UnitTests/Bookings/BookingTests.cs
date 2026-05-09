using Bookify.Domain.Bookings.Entities;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Bookings.Services;
using Bookify.Domain.Bookings.ValuesObjects;
using Bookify.Domain.Shared.ValuesObjects;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users.Entities;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact(DisplayName =
        $"{nameof(Booking)} {nameof(Booking.Reserve)} should raise {nameof(BookingReservedDomainEvent)}")]
    public void Reserve_Should_RaiseBookingReserveDomainEvent()
    {
        // Arrange
        var user = User.Create(
            UserData.FirstName,
            UserData.LastName,
            UserData.Email);

        var price = new Money(10.0m, Currency.Usd);

        var period = DateRange.Create(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 31));

        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();

        // Act
        var booking = Booking.Reserve(
            apartment,
            user.Id,
            period,
            DateTime.UtcNow,
            pricingService);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }
}