using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimuladorCajero.Pages;

public class MisTarjetasModel : PageModel
{
    public VirtualCard ActiveCreditCard { get; private set; } = default!;
    public VirtualCard DebitCard { get; private set; } = default!;
    public IReadOnlyList<VirtualCard> AvailableCreditCards { get; private set; } = Array.Empty<VirtualCard>();
    public IReadOnlyList<CreditTransactionSummary> CreditTransactions { get; private set; } = Array.Empty<CreditTransactionSummary>();

    public void OnGet()
    {
        var credits = new List<VirtualCard>
        {
            new(
                Alias: "Green",
                Brand: "AMEX",
                Holder: "JANE DOE",
                Number: "3759 8765 4321 0001",
                LastDigits: "0001",
                Expiry: "12/26",
                Cvv: "456",
                Type: CardType.Credit),
            new(
                Alias: "Reserve",
                Brand: "MAZE",
                Holder: "JANE DOE",
                Number: "5241 8888 9900 1444",
                LastDigits: "1444",
                Expiry: "05/27",
                Cvv: "301",
                Type: CardType.Credit)
        };

        AvailableCreditCards = credits;
        ActiveCreditCard = credits.First();

        DebitCard = new VirtualCard(
            Alias: "Cuenta Principal",
            Brand: "MAZE",
            Holder: "JANE DOE",
            Number: "4580 9911 2000 6732",
            LastDigits: "6732",
            Expiry: "--/--",
            Cvv: "000",
            Type: CardType.Debit);

    CreditTransactions = BuildCreditTransactions();
    }

    public record VirtualCard(
        string Alias,
        string Brand,
        string Holder,
        string Number,
        string LastDigits,
        string Expiry,
        string Cvv,
        CardType Type)
    {
        public string MaskedNumber => $"**** **** **** {LastDigits}";
    }

    public enum CardType
    {
        Credit,
        Debit
    }

    public sealed record CreditTransactionSummary(
        string Concepto,
        decimal Monto,
        int Cuotas,
        decimal RatePerMonth)
    {
        public decimal RatePercent => RatePerMonth * 100m;
        public decimal TotalInterest => Math.Round(Monto * RatePerMonth * Cuotas, 2, MidpointRounding.AwayFromZero);
        public decimal TotalAPagar => Monto + TotalInterest;
        public decimal PagoMensual => Cuotas > 0
            ? Math.Round(TotalAPagar / Cuotas, 2, MidpointRounding.AwayFromZero)
            : TotalAPagar;
    }

    private static IReadOnlyList<CreditTransactionSummary> BuildCreditTransactions()
    {
        return new[]
        {
            CreateSummary("Compra mercado mensual", 450_000m, 2),
            CreateSummary("Viaje familiar a Cartagena", 2_500_000m, 6),
            CreateSummary("Laptop para estudios", 4_800_000m, 10),
            CreateSummary("Emergencia médica", 1_200_000m, 4)
        };
    }

    private static CreditTransactionSummary CreateSummary(string concepto, decimal monto, int cuotas)
    {
        var rate = DetermineMonthlyRate(cuotas);
        return new CreditTransactionSummary(concepto, monto, cuotas, rate);
    }

    private static decimal DetermineMonthlyRate(int cuotas)
    {
        if (cuotas <= 2)
        {
            return 0m;
        }

        if (cuotas <= 6)
        {
            return 0.019m;
        }

        return 0.023m;
    }
}
