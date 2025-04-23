namespace VATCalculator.Domain.Queries
{
    public sealed class GetCalculationResult
    {
        public double NetAmmount { get; set; }
        public double GrossAmount { get; set; }
        public double VatTaxAmmount { get; set; }

        public GetCalculationResult()
        {
            NetAmmount = 0;
            GrossAmount = 0;
            VatTaxAmmount = 0;
        }

        public GetCalculationResult(double netAmmount, double grossAmount, double vatTaxAmmount)
        {
            NetAmmount = Math.Round(netAmmount, 2, MidpointRounding.AwayFromZero);
            GrossAmount = Math.Round(grossAmount, 2, MidpointRounding.AwayFromZero); 
            VatTaxAmmount = Math.Round(vatTaxAmmount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
