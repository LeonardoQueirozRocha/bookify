using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Application.Bookings.ConfirmBooking.Command;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Interfaces;
using Bookify.Domain.Bookings.Errors;
using Bookify.Domain.Bookings.Interfaces;

namespace Bookify.Application.Bookings.ConfirmBooking.Handlers;

public sealed class ConfirmBookingCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmBookingCommand>
{
    public async Task<Result> Handle(
        ConfirmBookingCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.NotFound);
        }

        var result = booking.Confirm(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}