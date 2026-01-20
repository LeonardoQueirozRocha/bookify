using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Application.Bookings.ReserveBooking.Commands;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Interfaces;
using Bookify.Domain.Apartments.Entities;
using Bookify.Domain.Apartments.Errors;
using Bookify.Domain.Apartments.Interfaces;
using Bookify.Domain.Bookings.Entities;
using Bookify.Domain.Bookings.Errors;
using Bookify.Domain.Bookings.Interfaces;
using Bookify.Domain.Bookings.Services;
using Bookify.Domain.Bookings.ValuesObjects;
using Bookify.Domain.Users.Errors;
using Bookify.Domain.Users.Interfaces;

namespace Bookify.Application.Bookings.ReserveBooking.Handlers;

internal sealed class ReserveBookingCommandHandler(
    IUserRepository userRepository,
    IApartmentRepository apartmentRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    PricingService pricingService,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<ReserveBookingCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var apartment = await apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        return await ReserveBookingAsync(
            apartment,
            user.Id,
            duration,
            cancellationToken);
    }

    #region Private methods

    private async Task<Result<Guid>> ReserveBookingAsync(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        CancellationToken cancellationToken)
    {
        try
        {
            var booking = Booking.Reserve(
                apartment,
                userId,
                duration,
                dateTimeProvider.UtcNow,
                pricingService);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }

    #endregion
}