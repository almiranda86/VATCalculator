using FluentValidation;
using FluentValidation.Results;
using Moq;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Queries;
using VATCalculator.Domain.Strategy;
using VATCalculator.Service.Managers;

namespace VATCalculator.Tests
{
    public class CalculationManagerTests
    {
        private readonly Mock<IValidator<GetCalculationRequest>> _validatorMock;
        private readonly Mock<ICalculateVATResolver> _resolverMock;
        private readonly CalculationManager _calculationManager;

        public CalculationManagerTests()
        {
            _validatorMock = new Mock<IValidator<GetCalculationRequest>>();
            _resolverMock = new Mock<ICalculateVATResolver>();
            _calculationManager = new CalculationManager(_validatorMock.Object, _resolverMock.Object);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenCalculatingFromNetAmount()
        {
            // Arrange
            var request = new GetCalculationRequest { NetAmmount = "100", VatRate = "20" };
            var cancellationToken = CancellationToken.None;

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var calculateVATMock = new Mock<ICalculateVAT>();
            calculateVATMock.Setup(c => c.Calculate("100", "20"))
                .Returns((120m, 100m, 20m));

            _resolverMock.Setup(r => r.Resolver(EVatCalculationMethods.NetAmmount))
                .Returns(new NetAmmountCalculationManager());

            // Act
            var result = await _calculationManager.HandleCalculation(request, cancellationToken);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenCalculatingFromGrossAmount()
        {
            // Arrange
            var request = new GetCalculationRequest { GrossAmount = "120", VatRate = "20" };
            var cancellationToken = CancellationToken.None;

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var calculateVATMock = new Mock<ICalculateVAT>();
            calculateVATMock.Setup(c => c.Calculate("120", "20"))
                .Returns((100m, 120m, 20m));

            _resolverMock.Setup(r => r.Resolver(EVatCalculationMethods.GrossAmmount))
                .Returns(new GrossAmmountCalculationManager());

            // Act
            var result = await _calculationManager.HandleCalculation(request, cancellationToken);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnResult_WhenCalculatingFromVatTaxAmount()
        {
            // Arrange
            var request = new GetCalculationRequest { VatTaxAmmount = "20", VatRate = "20" };
            var cancellationToken = CancellationToken.None;

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var calculateVATMock = new Mock<ICalculateVAT>();
            calculateVATMock.Setup(c => c.Calculate("20", "20"))
                .Returns((100m, 120m, 20m));

            _resolverMock.Setup(r => r.Resolver(EVatCalculationMethods.VatTaxAmmount))
                .Returns(new VatTaxAmmountCalculationManager());

            // Act
            var result = await _calculationManager.HandleCalculation(request, cancellationToken);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldReturnError_WhenNoValidCalculationMethod()
        {
            // Arrange
            var request = new GetCalculationRequest { VatRate = "20" };
            var cancellationToken = CancellationToken.None;

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _calculationManager.HandleCalculation(request, cancellationToken);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message.Contains("Something went wrong"));
        }
    }
}
