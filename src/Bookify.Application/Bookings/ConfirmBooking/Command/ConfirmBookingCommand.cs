using Bookify.Application.Abstractions.Messaging.Commands;

namespace Bookify.Application.Bookings.ConfirmBooking.Command;

public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;