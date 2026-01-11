namespace Bookify.Domain.Shared.ValuesObjects;

public record Currency
{
    internal static readonly Currency None = new(string.Empty);
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Brl = new("BRL");
    public static readonly IReadOnlyCollection<Currency> All = [Usd, Eur, Brl];

    private Currency(string code) => 
        Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code) =>
        All.FirstOrDefault(currency => currency.Code == code) ??
        throw new ApplicationException("The currency code is invalid");
}