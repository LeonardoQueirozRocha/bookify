using Bookify.Application.Apartments.SearchApartments.Queries;
using Bookify.Application.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace Bookify.Application.IntegrationTests.Apartments;

public class SearchApartmentTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task SearchApartments_ShouldReturnEmptyList_WhenDateRangeIsInvalid()
    {
        // Arrange
        var query = new SearchApartmentsQuery(
            new DateOnly(2026, 1, 10),
            new DateOnly(2026, 1, 1));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchApartments_ShouldReturnApartments_WhenDateRangeIsValid()
    {
        // Arrange
        var query = new SearchApartmentsQuery(
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 10));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}