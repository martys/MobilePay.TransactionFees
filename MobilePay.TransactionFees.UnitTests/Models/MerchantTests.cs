using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.Models
{
    public class MerchantTests
    {
        [Fact]
        public void ApplyTransactionPercentageFeeDiscount_FeeNull_ThrowsException()
        {
            //arrange
            var merchant = new Merchant(new Name("STEAM"), new Percentage(10));
            
            //act & assert
            Assert.Throws<DomainException>(() => merchant.ApplyTransactionPercentageFeeDiscount(null));
        }

        [Theory]
        [InlineData(10, 10, 9)]
        [InlineData(0, 10, 0)]
        [InlineData(10, 0, 10)]
        public void ApplyTransactionPercentageFeeDiscount_CalculatesFeeCorrectly(double originalFeeAmount, 
            double discount, double expectedFeeAmount)
        {
            //arrange
            var merchant = new Merchant(new Name("STEAM"), new Percentage(discount));
            var originalFee = new Fee(originalFeeAmount);
            
            //act
            var fee = merchant.ApplyTransactionPercentageFeeDiscount(originalFee);
            
            //assert
            Assert.Equal(expectedFeeAmount, fee.Value);
        }
    }
}