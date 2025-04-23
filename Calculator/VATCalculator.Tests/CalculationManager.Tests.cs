using FluentValidation;
using FluentValidation.Results;
using Moq;
using VATCalculator.Domain.Queries;
using VATCalculator.Service.Managers;
using VATCalculator.Service.Validations;

namespace VATCalculator.Tests
{
    public class CalculationManagerTests
    {
        private readonly Mock<IValidator<GetCalculationRequest>> _validatorMock;
        private readonly CalculationManager _calculationManager;
        private readonly IValidator<GetCalculationRequest> _validator;

        public CalculationManagerTests()
        {
            _validatorMock = new Mock<IValidator<GetCalculationRequest>>();
            _calculationManager = new CalculationManager(_validatorMock.Object);
            _validator = new GetCalculationRequestValidation();
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenNetAmountProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                NetAmmount = "100",
                VatRate = "20"
            };
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _calculationManager.HandleCalculation(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenGrossAmountProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                GrossAmount = "120",
                VatRate = "20"
            };
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _calculationManager.HandleCalculation(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenVatTaxAmountProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                VatTaxAmmount = "20",
                VatRate = "20"
            };
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _calculationManager.HandleCalculation(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }
    }
}
