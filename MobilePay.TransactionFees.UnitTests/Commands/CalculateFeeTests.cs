using System;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.Commands
{
    public class CalculateFeeTests
    {
        [Fact]
        public void Constructor_EmptyCorellationId_ThrowsException()
        {
            //arrange
            var transaction = new Transaction(new Date(DateTime.Now), new Name("STEAM"), new Amount(100));
            
            //act & assert
            Assert.Throws<DomainException>(() => new CalculateFee(Guid.Empty, transaction));
        }

        [Fact]
        public void Constructor_NullTransaction_ThrowsException()
        {
            //act & assert
            Assert.Throws<DomainException>(() => new CalculateFee(Guid.NewGuid(), null));
        }
    }
}