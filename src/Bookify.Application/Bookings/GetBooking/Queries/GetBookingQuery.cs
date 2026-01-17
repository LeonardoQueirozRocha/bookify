using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Application.Bookings.GetBooking.Responses;

namespace Bookify.Application.Bookings.GetBooking.Queries;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;