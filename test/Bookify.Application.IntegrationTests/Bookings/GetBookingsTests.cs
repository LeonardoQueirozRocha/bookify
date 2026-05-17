using Bookify.Application.Bookings.GetBooking.Queries;
using Bookify.Application.IntegrationTests.Infrastructure;
using Bookify.Domain.Bookings.Errors;
using FluentAssertions;

namespace Bookify.Application.IntegrationTests.Bookings;

public class GetBookingsTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly Guid BookingId = Guid.NewGuid();

    [Fact]
    public async Task GetBooking_ShouldReturnFailure_WhenBookingIsNotFound()
    {
        // Arrange
        var query = new GetBookingQuery(BookingId);

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.Error.Should().Be(BookingErrors.NotFound);
    }
}