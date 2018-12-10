using System;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.CommandHandlers
{
    public class CalculateFeeHandler : ICommandHandler<CalculateFee, Fee>
    {
        private readonly Percentage _transactionPercentageFee;
        
        public CalculateFeeHandler(Percentage transactionPercentageFee)
        {
            _transactionPercentageFee = transactionPercentageFee 
                ?? throw new ApplicationException("Transaction percentage cannot be null");
        }
        
        public Fee Handle(CalculateFee command)
        {
            if (command == null)
            {
                throw new DomainException($"Command cannot be null");
            }
            
            return command.Transaction.CalculateTransactionPercentageFee(_transactionPercentageFee);
        }
    }
}