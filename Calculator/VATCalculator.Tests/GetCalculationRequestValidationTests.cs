using FluentValidation.TestHelper;
using VATCalculator.Domain.Queries;
using VATCalculator.Service.Validations;

namespace VATCalculator.Tests
{
    public class GetCalculationRequestValidationTests
    {
        private readonly GetCalculationRequestValidation _validator;

        public GetCalculationRequestValidationTests()
        {
            _validator = new GetCalculationRequestValidation();
        }

        [Fact]
        public void Should_Have_Error_When_Multiple_Values_Provided()
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = "100",
                GrossAmount = "200",
                VatTaxAmmount = "50"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage("Only 1 of 3 of the values must be provided. - Net Ammount | Gross Amount | Vat Tax Ammount");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_Value_Provided()
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = "100",
                GrossAmount = string.Empty,
                VatTaxAmmount = string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Theory]
        [InlineData("abc", "GrossAmount")]
        [InlineData("123.45.67B", "NetAmmount")]
        [InlineData("12a3", "VatTaxAmmount")]
        [InlineData("rate%", "VatRate")]
        public void Should_Have_Error_When_Value_Is_Not_Numeric(string value, string propertyName)
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = propertyName == "NetAmmount" ? value : string.Empty,
                GrossAmount = propertyName == "GrossAmount" ? value : string.Empty,
                VatTaxAmmount = propertyName == "VatTaxAmmount" ? value : string.Empty,
                VatRate = propertyName == "VatRate" ? value : string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage($"{propertyName} needs to be a number");
        }

        [Theory]
        [InlineData("-1", "NetAmmount")]
        [InlineData("0", "GrossAmount")]
        [InlineData("-100", "VatTaxAmmount")]
        [InlineData("0", "VatRate")]
        public void Should_Have_Error_When_Value_Is_Not_Greater_Than_Zero(string value, string propertyName)
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = propertyName == "NetAmmount" ? value : string.Empty,
                GrossAmount = propertyName == "GrossAmount" ? value : string.Empty,
                VatTaxAmmount = propertyName == "VatTaxAmmount" ? value : string.Empty,
                VatRate = propertyName == "VatRate" ? value : string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage($"{propertyName} needs to be greater than Zero");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Values_Are_Valid()
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = "100",
                GrossAmount = string.Empty,
                VatTaxAmmount = string.Empty,
                VatRate = "20"
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Theory]
        [InlineData("21", "VatRate")]
        [InlineData("11", "VatRate")]
        [InlineData("12", "VatRate")]
        public void Should_Have_Error_When_Value_Is_Not_Between_10_13_20(string value, string propertyName)
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = string.Empty,
                GrossAmount = string.Empty,
                VatTaxAmmount = string.Empty,
                VatRate = propertyName == "VatRate" ? value : string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage($"{propertyName} must be 10, 13 or 20");
        }


        [Theory]
        [InlineData("20", "VatRate")]
        [InlineData("10", "VatRate")]
        [InlineData("13", "VatRate")]
        public void Should_Not_Have_Error_When_Value_Is_Between_10_13_20(string value, string propertyName)
        {
            var request = new GetCalculationRequest
            {
                NetAmmount = string.Empty,
                GrossAmount = string.Empty,
                VatTaxAmmount =  string.Empty,
                VatRate = propertyName == "VatRate" ? value : string.Empty
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public async Task Should_Throw_Validation_Exception_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new GetCalculationRequest();

            // Use TestValidate to validate the request
            var validationResult = _validator.TestValidate(request);

            // Act & Assert
            validationResult.ShouldHaveValidationErrorFor(request => request);
        }
    }
}
