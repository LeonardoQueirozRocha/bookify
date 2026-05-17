using Bookify.Domain.Abstractions.Interfaces;

namespace Bookify.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;