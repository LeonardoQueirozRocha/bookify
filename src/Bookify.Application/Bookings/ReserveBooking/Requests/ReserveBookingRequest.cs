namespace Bookify.Application.Bookings.ReserveBooking.Requests;

public sealed record ReserveBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);