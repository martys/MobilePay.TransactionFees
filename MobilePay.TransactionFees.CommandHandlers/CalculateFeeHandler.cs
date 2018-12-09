using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.CommandHandlers
{
    public class CalculateFeeHandler : ICommandHandler<CalculateFee, Fee>
    {
        private readonly Percentage _transactionPercentageFee;
        
        public CalculateFeeHandler(Percentage transactionPercentageFee)
        {
            _transactionPercentageFee = transactionPercentageFee;
        }
        
        public Fee Handle(CalculateFee command)
        {
            return command.Transaction.CalculateTransactionPercentageFee(_transactionPercentageFee);
        }
    }
}