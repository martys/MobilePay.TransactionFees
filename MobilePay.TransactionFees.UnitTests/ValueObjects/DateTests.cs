using System;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.ValueObjects
{
    public class DateTests
    {
        [Fact]
        public void Constructor_CreatesValidInstace()
        {
            //arrange
            var dateTime = new DateTime(2007, 11, 11);
            
            //act
            var date = new Date(dateTime);

            //assert
            Assert.Equal(dateTime, date.Value);
        }

        [Fact]
        public void MonthsMatch_OtherNull_ReturnsFalse()
        {
            //arrange
            var date = new Date(DateTime.Parse("1989-11-26"));
            
            //act % assert
            Assert.False(date.MonthsMatch(null));
        }

        [Theory]
        [InlineData("1989-11-26", "1989-11-26")]
        [InlineData("1989-11-26", "1989-11-11")]
        public void MonthsMatch_IfMatches_ReturnsTrue(string stringDate1, string stringDate2)
        {
            //arrange
            var date1 = new Date(DateTime.Parse(stringDate1));
            var date2 = new Date(DateTime.Parse(stringDate2));
            
            //act & assert
            Assert.True(date1.MonthsMatch(date2));
        }

        [Theory]
        [InlineData("1989-11-26", "1989-12-26")]
        [InlineData("1989-11-26", "1990-11-26")]
        public void MonthsMatch_IfDoesntMatch_ReturnsFalse(string stringDate1, string stringDate2)
        {
            //arrange
            var date1 = new Date(DateTime.Parse(stringDate1));
            var date2 = new Date(DateTime.Parse(stringDate2));
            
            //act & assert
            Assert.False(date1.MonthsMatch(date2));
        }
    }
}