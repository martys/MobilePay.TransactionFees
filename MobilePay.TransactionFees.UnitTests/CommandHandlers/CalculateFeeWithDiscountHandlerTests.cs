using System;
using System.Runtime.InteropServices;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.Repositories;
using MobilePay.TransactionFees.Domain.ValueObjects;
using MobilePay.TransactionFees.InMemoryStorage;
using Moq;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.CommandHandlers
{
    public class CalculateFeeWithDiscountHandlerTests
    {
        [Fact]
        public void Constructor_NullHandler_ThrowsException()
        {
            //act & assert
            Assert.Throws<ApplicationException>(
                () => new CalculateFeeWithDiscountHandler(null, new InMemoryMerchantRepository()));
        }

        [Fact]
        public void Constructor_NullMerchantRepository_ThrowsException()
        {
            //arrange
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            
            //act & assert
            Assert.Throws<ApplicationException>(
                () => new CalculateFeeWithDiscountHandler(calculateFeeHandler.Object, null));
        }

        [Fact]
        public void Handle_NonExistentMerchant_ThrowsException()
        {
            //arrange
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            var merchantRepository = new Mock<IMerchantRepository>();
            merchantRepository.Setup(x => x.Get(It.IsAny<Name>())).Returns((Merchant)null);
            var calculateFeeWithDiscountHandler = new CalculateFeeWithDiscountHandler(calculateFeeHandler.Object,
                merchantRepository.Object);
            var transaction = new Transaction(new Date(DateTime.Now), new Name("STEAM"), new Amount(100));

            //act && assert
            Assert.Throws<DomainObjectNotFoundException>(
                () => calculateFeeWithDiscountHandler.Handle(new CalculateFee(Guid.NewGuid(), transaction)));
        }

        [Theory]
        [InlineData(1, 10, 0.9)]
        [InlineData(100, 0, 100)]
        [InlineData(0, 50, 0)]
        public void Handle_AppliesMerchantDiscount(double originalFee, double merchantDiscount, double expectedFee)
        {
            //arrange
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            calculateFeeHandler.Setup(x => x.Handle(It.IsAny<CalculateFee>())).Returns(new Fee(originalFee));
            var merchantRepository = new Mock<IMerchantRepository>();
            merchantRepository.Setup(x => x.Get(It.IsAny<Name>())).
                Returns(new Merchant(new Name("STEAM"), new Percentage(merchantDiscount)));
            var calculateFeeWithDiscountHandler = new CalculateFeeWithDiscountHandler(calculateFeeHandler.Object,
                merchantRepository.Object);
            var transaction = new Transaction(new Date(DateTime.Now), new Name("STEAM"), new Amount(100));
            
            //act
            var feeWithDiscount = calculateFeeWithDiscountHandler.Handle(new CalculateFee(Guid.NewGuid(), transaction));
            
            //assert
            calculateFeeHandler.Verify(x => x.Handle(It.IsAny<CalculateFee>()), Times.Once);
            Assert.Equal(expectedFee, feeWithDiscount.Value);
        }
    }
}