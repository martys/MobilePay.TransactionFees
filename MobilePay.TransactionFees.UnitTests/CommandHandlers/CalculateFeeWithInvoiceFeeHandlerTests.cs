using System;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Moq;
using Xunit;

namespace MobilePay.TransactionFees.UnitTests.CommandHandlers
{
    public class CalculateFeeWithInvoiceFeeHandlerTests
    {
        [Fact]
        public void Constructor_NullHandler_ThrowsException()
        {
            //act & assert
            Assert.Throws<ApplicationException>(
                () => new CalculateFeeWithInvoiceFeeHandler(null, new Fee(29)));
        }

        [Fact]
        public void Constructor_NullFee_ThrowsException()
        {
            //arrange
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            
            //act & assert
            Assert.Throws<ApplicationException>(
                () => new CalculateFeeWithInvoiceFeeHandler(calculateFeeHandler.Object, null));
        }

        [Fact]
        public void Handle_OriginalFeeZero_DoesntAddInvoiceFee()
        {
            //arrange
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            calculateFeeHandler.Setup(x => x.Handle(It.IsAny<CalculateFee>())).Returns(new Fee(0));
            var calculateFeeWithInvoiceHandler = new CalculateFeeWithInvoiceFeeHandler(
                calculateFeeHandler.Object, new Fee(10));
            var transaction = new Transaction(new Date(DateTime.Now), new Name("STEAM"), new Amount(100));
            
            //act
            var feeWithInvoice = calculateFeeWithInvoiceHandler.Handle(new CalculateFee(Guid.NewGuid(), transaction));
            
            //assert
            Assert.Equal(0, feeWithInvoice.Value);
        }

        [Fact]
        public void Handle_TwoTransactionsSameMonth_OnlyAddsFeeToFirstOne()
        {
            //arrange
            var transactionFee = new Fee(10);
            var calculateFeeHandler = new Mock<ICommandHandler<CalculateFee, Fee>>();
            calculateFeeHandler.Setup(x => x.Handle(It.IsAny<CalculateFee>())).Returns(transactionFee);
            var fixedInvoiceFee = new Fee(29);
            var calculateFeeWithInvoiceHandler = new CalculateFeeWithInvoiceFeeHandler(
                calculateFeeHandler.Object, fixedInvoiceFee);
            var transaction1 = new Transaction(
                new Date(DateTime.Parse("1989-11-26")), new Name("STEAM"), new Amount(100));
            var transaction2 = new Transaction(
                new Date(DateTime.Parse("1989-11-27")), new Name("STEAM"), new Amount(100));
            
            //act
            var feeWithInvoice = calculateFeeWithInvoiceHandler.Handle(new CalculateFee(Guid.NewGuid(), transaction1));
            var feeWithoutInvoice = calculateFeeWithInvoiceHandler.Handle(new CalculateFee(Guid.NewGuid(), transaction2));
            
            //assert
            Assert.Equal(fixedInvoiceFee.Value + transactionFee.Value, feeWithInvoice.Value);
            Assert.Equal(transactionFee.Value, feeWithoutInvoice.Value);
        }
    }
}