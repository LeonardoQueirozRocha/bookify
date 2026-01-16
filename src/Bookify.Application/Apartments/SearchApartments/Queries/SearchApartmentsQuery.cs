using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Application.Apartments.SearchApartments.Responses;

namespace Bookify.Application.Apartments.SearchApartments.Queries;

public record SearchApartmentsQuery(
    DateOnly StartDate,
    DateOnly EndDate)
    : IQuery<IReadOnlyList<ApartmentResponse>>;