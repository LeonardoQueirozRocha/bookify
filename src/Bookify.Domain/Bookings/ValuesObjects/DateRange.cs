namespace Bookify.Domain.Bookings.ValuesObjects;

public record DateRange
{
    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public DateOnly Start { get; init; }

    public DateOnly End { get; init; }

    public int LenghtInDays => End.DayNumber - Start.DayNumber;

    public static DateRange Create(DateOnly start, DateOnly end) =>
        start > end
            ? throw new ApplicationException("End date precedes start date")
            : new DateRange(start, end);
}