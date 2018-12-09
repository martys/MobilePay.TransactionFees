using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.ValueObjects
{
    public class AmountTests
    {
        [Theory]
        [InlineData(2.12)]
        [InlineData(1)]
        [InlineData(25.47785)]
        public void Constructor_PositiveNumber_CreatesValidInstance(double input)
        {
            //act
            var amount = new Amount(input);
            
            //assert
            Assert.Equal(input, amount.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1.25)]
        public void Constructor_NonPositiveNumber_ThrowsException(double input)
        {
            //act & assert
            Assert.Throws<DomainException>(() => new Amount(input));
        }
    }
}