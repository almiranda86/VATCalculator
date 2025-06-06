﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using VATCalculator.Api.Extensions;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Queries;

namespace VATCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculationManager _calculationService;

        public CalculatorController(ICalculationManager calculationService)
        {
            _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
        }

        /// <summary>
        /// Given 1 of the 3 values - Net Ammount | Gross Ammount | VAT Tax Rate - calculate the other 2. A Tax Rate should be provided to run the calculation.
        /// </summary>
        /// <param name="netAmmount">The Net Ammount for calculation</param>
        /// <param name="grossAmount">The Gross Amount for  calculation</param>
        /// <param name="vatTaxAmmount">The VAT Tax Amount for  calculation</param>
        /// <param name="vatRate">For Austria, is expected: 10, 13 or 20</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// An object of type GetCalculationResult which contains the calculated values.
        /// </returns>
        [HttpGet("/calculate-vat")]
        [ProducesResponseType(typeof(GetCalculationRequest), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 500)]
        public async Task<IActionResult> Get([FromQuery] string? netAmmount,
                                             [FromQuery] string? grossAmount,
                                             [FromQuery] string? vatTaxAmmount,
                                             [FromQuery] string? vatRate,
                                             CancellationToken cancellationToken = default)
        {
            var request = new GetCalculationRequest(netAmmount, grossAmount, vatTaxAmmount, vatRate);

            var response = await _calculationService.HandleCalculation(request, cancellationToken);

            if (response.IsFailed)
                return this.ApiResult(HttpStatusCode.InternalServerError, response.Errors);

            return this.ApiResult(HttpStatusCode.OK, response.ResponseModel);
        }
    }
}
