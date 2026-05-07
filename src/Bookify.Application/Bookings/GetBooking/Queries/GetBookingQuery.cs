using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Bookings.GetBooking.Responses;

namespace Bookify.Application.Bookings.GetBooking.Queries;

public sealed record GetBookingQuery(Guid BookingId) : ICachedQuery<BookingResponse>
{
    public string CacheKey =>
        $"bookings-{BookingId}";

    public TimeSpan? Expiration =>
        null;
}