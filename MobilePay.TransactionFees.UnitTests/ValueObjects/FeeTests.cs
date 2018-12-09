using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.ValueObjects
{
    public class FeeTests
    {
        [Theory]
        [InlineData(25)]
        [InlineData(0)]
        public void Constructor_NonNegativeNumber_CreatesValidInstance(double input)
        {
            //act
            var invoiceFee = new Fee(input);
            
            //assert
            Assert.Equal(input, invoiceFee.Value);
        }

        [Theory]
        [InlineData(-0.001)]
        [InlineData(-100)]
        public void Constructor_NegativeNumber_ThrowsException(double input)
        {
            //act & assert
            Assert.Throws<DomainException>(() => new Fee(input));
        }

        [Theory]
        [InlineData(2.00000)]
        [InlineData(2.00321)]
        [InlineData(2)]
        public void ToString_ShouldFormatNumbersCorrectly(double input)
        {
            //act
            var fee = new Fee(input);
            
            //assert
            Assert.Equal("2.00", fee.ToString());
        }
    }
}