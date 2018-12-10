using System;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.CommandHandlers
{
    public class CalculateFeeHandlerTests
    {
        [Fact]
        public void Constructor_NullFee_ThrowsException()
        {
            //act & assert
            Assert.Throws<ApplicationException>(() => new CalculateFeeHandler(null));
        }
        
        [Fact]
        public void Handle_NullTransaction_ThrowsException()
        {
            //arrange
            var handler = new CalculateFeeHandler(new Percentage(10));
            
            //act & assert
            Assert.Throws<DomainException>(() => handler.Handle(null));
        }

        [Theory]
        [InlineData(100, 0, 0)]
        [InlineData(100, 10, 10)]
        [InlineData(1, 10, 0.1)]
        public void Handle_CalculatesFeeCorrectly(double amount, double percentage, double expectedFee)
        {
            //arrange
            var handler = new CalculateFeeHandler(new Percentage(percentage));
            var transaction = new Transaction(new Date(DateTime.Now), new Name("STEAM"), new Amount(amount));
            
            //act
            var fee = handler.Handle(new CalculateFee(Guid.NewGuid(), transaction));
            
            //assert
            Assert.Equal(expectedFee, fee.Value);
        }
    }
}