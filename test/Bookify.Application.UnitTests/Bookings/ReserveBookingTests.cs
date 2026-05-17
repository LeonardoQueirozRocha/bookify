using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Bookings.ReserveBooking.Commands;
using Bookify.Application.Bookings.ReserveBooking.Handlers;
using Bookify.Application.Exceptions;
using Bookify.Application.UnitTests.Apartments;
using Bookify.Application.UnitTests.Users;
using Bookify.Domain.Abstractions.Interfaces;
using Bookify.Domain.Apartments.Entities;
using Bookify.Domain.Apartments.Errors;
using Bookify.Domain.Apartments.Interfaces;
using Bookify.Domain.Bookings.Entities;
using Bookify.Domain.Bookings.Errors;
using Bookify.Domain.Bookings.Interfaces;
using Bookify.Domain.Bookings.Services;
using Bookify.Domain.Bookings.ValuesObjects;
using Bookify.Domain.Users.Entities;
using Bookify.Domain.Users.Errors;
using Bookify.Domain.Users.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit.Sdk;

namespace Bookify.Application.UnitTests.Bookings;

public class ReserveBookingTests
{
    private static readonly DateTime UtcNow = DateTime.UtcNow;
    private static readonly CancellationToken CancellationToken = CancellationToken.None;

    private static readonly ReserveBookingCommand Command = new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        new DateOnly(2026, 1, 1),
        new DateOnly(2026, 1, 10));

    private readonly ReserveBookingCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IApartmentRepository _apartmentRepositoryMock;
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly PricingService _pricingServiceMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    public ReserveBookingTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _apartmentRepositoryMock = Substitute.For<IApartmentRepository>();
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        _dateTimeProviderMock.UtcNow.Returns(UtcNow);

        _handler = new ReserveBookingCommandHandler(
            _userRepositoryMock,
            _apartmentRepositoryMock,
            _bookingRepositoryMock,
            _unitOfWorkMock,
            new PricingService(),
            _dateTimeProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUserIsNull()
    {
        // Arrange
        User? user = null;
        var expectedError = UserErrors.NotFound;

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenApartmentIsNull()
    {
        // Arrange
        Apartment? apartment = null;
        var user = UserData.Create();
        var expectedError = ApartmentErrors.NotFound;

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _apartmentRepositoryMock
            .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
            .Returns(apartment);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenApartmentIsBooked()
    {
        // Arrange
        const bool isOverlapping = true;

        var user = UserData.Create();
        var apartment = ApartmentData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);
        var expectedError = BookingErrors.Overlap;

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _apartmentRepositoryMock
            .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
            .Returns(apartment);

        _bookingRepositoryMock
            .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
            .Returns(isOverlapping);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUnitOfWorkThrows()
    {
        // Arrange
        const bool isOverlapping = false;

        var user = UserData.Create();
        var apartment = ApartmentData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);
        var expectedError = BookingErrors.Overlap;
        var concurrencyException = new ConcurrencyException("Concurrency exception", new Exception());

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _apartmentRepositoryMock
            .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
            .Returns(apartment);

        _bookingRepositoryMock
            .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
            .Returns(isOverlapping);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(concurrencyException);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenBookingIsReserved()
    {
        // Arrange
        const bool isOverlapping = false;

        var user = UserData.Create();
        var apartment = ApartmentData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _apartmentRepositoryMock
            .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
            .Returns(apartment);

        _bookingRepositoryMock
            .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
            .Returns(isOverlapping);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenBookingIsReserved()
    {
        // Arrange
        const bool isOverlapping = false;

        var user = UserData.Create();
        var apartment = ApartmentData.Create();
        var duration = DateRange.Create(Command.StartDate, Command.EndDate);

        _userRepositoryMock
            .GetByIdAsync(Command.UserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _apartmentRepositoryMock
            .GetByIdAsync(Command.ApartmentId, Arg.Any<CancellationToken>())
            .Returns(apartment);

        _bookingRepositoryMock
            .IsOverlappingAsync(apartment, duration, Arg.Any<CancellationToken>())
            .Returns(isOverlapping);

        // Act
        var result = await _handler.Handle(Command, CancellationToken);

        // Assert
        _bookingRepositoryMock
            .Received(1)
            .Add(Arg.Is<Booking>(booking => booking.Id == result.Value));
    }
}