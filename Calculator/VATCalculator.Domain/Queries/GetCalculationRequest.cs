using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
