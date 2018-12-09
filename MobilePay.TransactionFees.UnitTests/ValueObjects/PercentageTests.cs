using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.ValueObjects
{
    public class PercentageTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(22.5)]
        [InlineData(100)]
        public void Constructor_ValidInput_CreatesValidInstance(double input)
        {
            //act
            var percentage = new Percentage(input);
            
            //assert
            Assert.Equal(input, percentage.Value);
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(101)]
        public void Constructor_InvalidInput_ThrowsException(double input)
        {
            //act & assert
            Assert.Throws<DomainException>(() => new Percentage(input));
        }
    }
}