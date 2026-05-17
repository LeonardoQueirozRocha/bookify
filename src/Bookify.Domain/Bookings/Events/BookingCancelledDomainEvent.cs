using Bookify.Domain.Abstractions.Interfaces;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent;