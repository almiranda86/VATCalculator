namespace VATCalculator.Domain.Queries
{
    public sealed class GetCalculationRequest
    {
        public string NetAmmount { get; set; }
        public string GrossAmount { get; set; }
        public string VatTaxAmmount { get; set; }
        public string VatRate { get; set; }

        public GetCalculationRequest()
        {
            NetAmmount = string.Empty;
            GrossAmount = string.Empty;
            VatTaxAmmount = string.Empty;
            VatRate = string.Empty;
        }

        public GetCalculationRequest(string netAmmount, string grossAmount, string vatTaxAmmount, string vatRate)
        {
            NetAmmount = netAmmount ?? string.Empty;
            GrossAmount = grossAmount ?? string.Empty;
            VatTaxAmmount = vatTaxAmmount ?? string.Empty;
            VatRate = vatRate ?? string.Empty;
        }
    }
}
