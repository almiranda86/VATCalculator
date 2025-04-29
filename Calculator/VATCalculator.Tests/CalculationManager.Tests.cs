using FluentValidation;
using FluentValidation.Results;
using Moq;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Queries;
using VATCalculator.Service.Managers;

namespace VATCalculator.Tests
{
    public class CalculationManagerTests
    {
        private readonly Mock<IValidator<GetCalculationRequest>> _validatorMock;
        private readonly Mock<INetAmmountCalculationManager> _netAmmountCalculationManagerMock;
        private readonly Mock<IGrossAmmountCalculationManager> _grossAmmountCalculationManagerMock;
        private readonly Mock<IVatTaxAmmountCalculationManager> _vatTaxAmmountCalculationManagerMock;
        private readonly CalculationManager _calculationManager;

        public CalculationManagerTests()
        {
            _validatorMock = new Mock<IValidator<GetCalculationRequest>>();
            _netAmmountCalculationManagerMock = new Mock<INetAmmountCalculationManager>();
            _grossAmmountCalculationManagerMock = new Mock<IGrossAmmountCalculationManager>();
            _vatTaxAmmountCalculationManagerMock = new Mock<IVatTaxAmmountCalculationManager>();

            _calculationManager = new CalculationManager(
                _validatorMock.Object,
                _netAmmountCalculationManagerMock.Object,
                _grossAmmountCalculationManagerMock.Object,
                _vatTaxAmmountCalculationManagerMock.Object
            );
        }

        [Fact]
        public async Task HandleCalculation_ShouldCalculateFromNetAmount_WhenNetAmountIsProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                NetAmmount = "100",
                VatRate = "20"
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _netAmmountCalculationManagerMock
                .Setup(m => m.CalculateFromNetAmmount("100", "20"))
                .Returns((100m, 120m, 20m));

            // Act
            var result = await _calculationManager.HandleCalculation(request, default);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldCalculateFromGrossAmount_WhenGrossAmountIsProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                GrossAmount = "120",
                VatRate = "20"
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _grossAmmountCalculationManagerMock
                .Setup(m => m.CalculateFromGrossAmmount("120", "20"))
                .Returns((100m, 120m, 20m));

            // Act
            var result = await _calculationManager.HandleCalculation(request, default);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }

        [Fact]
        public async Task HandleCalculation_ShouldCalculateFromVatTaxAmount_WhenVatTaxAmountIsProvided()
        {
            // Arrange
            var request = new GetCalculationRequest
            {
                VatTaxAmmount = "20",
                VatRate = "20"
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _vatTaxAmmountCalculationManagerMock
                .Setup(m => m.CalculateFromVatTaxAmmount("20", "20"))
                .Returns((100m, 120m, 20m));

            // Act
            var result = await _calculationManager.HandleCalculation(request, default);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(100, result.ResponseModel.NetAmmount);
            Assert.Equal(120, result.ResponseModel.GrossAmount);
            Assert.Equal(20, result.ResponseModel.VatTaxAmmount);
        }
    }
}
