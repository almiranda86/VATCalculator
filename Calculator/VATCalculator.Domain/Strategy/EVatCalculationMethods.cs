using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VATCalculator.Domain.Strategy
{
    public enum EVatCalculationMethods
    {
        GrossAmmount,
        VatTaxAmmount,
        NetAmmount
    }
}
