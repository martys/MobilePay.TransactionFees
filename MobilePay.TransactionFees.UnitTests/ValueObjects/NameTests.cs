using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.ValueObjects
{
    public class NameTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_InvalidInput_ThrowsException(string input)
        {
            //act & assert
            Assert.Throws<DomainException>(() => new Name(input));
        }

        [Fact]
        public void Constructor_ValidInput_CreatesValidInstance()
        {
            //arrange
            var nameString = "STEAM";
                
            //act
            var name = new Name(nameString);
            
            //act & assert
            Assert.Equal(nameString, name.Value);
        }
    }
}