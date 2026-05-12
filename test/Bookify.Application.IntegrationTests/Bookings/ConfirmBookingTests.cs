using Bookify.Application.Bookings.ConfirmBooking.Command;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Bookings.Errors;
using FluentAssertions;

namespace Bookify.Application.IntegrationTests.Bookings;

public class ConfirmBookingTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid BookingId = Guid.NewGuid();
    
    [Fact]
    public async Task ConfirmBooking_ShouldReturnFailure_WhenBookingIsNotFound()
    {
        // Arrange
        var query = new ConfirmBookingCommand(BookingId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(BookingErrors.NotFound);
    }
}