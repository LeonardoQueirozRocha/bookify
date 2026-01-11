namespace Bookify.Domain.Shared.ValuesObjects;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money first, Money second) =>
        first.Currency != second.Currency
            ? throw new InvalidOperationException("Currencies have to be equal")
            : new Money(first.Amount + second.Amount, first.Currency);

    public static Money Zero() => new(decimal.Zero, Currency.None);

    public static Money Zero(Currency currency) => new(decimal.Zero, currency);

    public bool IsZero() => this == Zero(Currency);
}