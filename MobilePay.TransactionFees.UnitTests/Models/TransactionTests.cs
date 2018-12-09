using System;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.Models
{
    public class TransactionTests
    {
        [Theory]
        [InlineData("1989-11-26", "1989-11-26")]
        [InlineData("1989-11-26", "1989-11-27")]
        public void HappenedOnSameMonth_True_ReturnsTrue(string stringDate1, string stringDate2)
        {
            //arrange
            var transaction1 = new Transaction(
                new Date(DateTime.Parse(stringDate1)), 
                new Name("STEAM"),
                new Amount(100));
            var transaction2 = new Transaction(
                new Date(DateTime.Parse(stringDate2)), 
                new Name("STEAM"),
                new Amount(100));
            
            //act & assert
            Assert.True(transaction1.HappenedOnSameMonth(transaction2));
        }

        [Theory] 
        [InlineData("1989-11-26", "1990-11-26")] 
        [InlineData("1989-11-26", "1989-12-26")]
        public void HappenedOnSameMonth_False_ReturnsFalse(string stringDate1, string stringDate2)
        {
            //arrange
            var transaction1 = new Transaction(
                new Date(DateTime.Parse(stringDate1)), 
                new Name("STEAM"),
                new Amount(100));
            var transaction2 = new Transaction(
                new Date(DateTime.Parse(stringDate2)), 
                new Name("STEAM"),
                new Amount(100));
            
            //act & assert
            Assert.False(transaction1.HappenedOnSameMonth(transaction2));
        }

        [Fact]
        public void CalculateTransactionPercentageFee_NullPercentage_ThrowsException()
        {
            //arrange
            var transaction = new Transaction(
                new Date(DateTime.Parse("1989-11-26")), 
                new Name("STEAM"),
                new Amount(100));
            
            //act & assert
            Assert.Throws<DomainException>(() => transaction.CalculateTransactionPercentageFee(null));
        }

        [Theory]
        [InlineData(100, 0, 0)]
        [InlineData(100, 10, 10)]
        [InlineData(100.20, 10, 10.02)]
        public void CalculateTransactionPercentageFee_CalculatesCorrectly(double amount, double percents,
            double expected)
        {
            //arrange
            var transaction = new Transaction(
                new Date(DateTime.Parse("1989-11-26")), 
                new Name("STEAM"),
                new Amount(amount));
            var percentage = new Percentage(percents);
            
            //act
            var fee = transaction.CalculateTransactionPercentageFee(percentage);
            
            //assert
            Assert.Equal(expected, fee.Value);
        }
    }
}