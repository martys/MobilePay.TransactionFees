using System.Collections.Generic;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.CommandHandlers
{
    public class CalculateFeeWithInvoiceFeeHandler : ICommandHandler<CalculateFee, Fee>
    {
        private readonly ICommandHandler<CalculateFee, Fee> _calculateFeeHandler;
        private readonly Fee _invoiceFixedFee;
        private readonly IDictionary<string, Transaction> _previousTransactions;

        public CalculateFeeWithInvoiceFeeHandler(ICommandHandler<CalculateFee, Fee> calculateFeeHandler,
            Fee invoiceFixedFee)
        {
            _calculateFeeHandler = calculateFeeHandler;
            _invoiceFixedFee = invoiceFixedFee;
            _previousTransactions = new Dictionary<string, Transaction>();
        }
        
        public Fee Handle(CalculateFee command)
        {
            var finalFee = _calculateFeeHandler.Handle(command);
            
            // two conditions have to be satisfied for merchant to be charged invoice fee:
            // it must be merchant's first transaction during this month
            // transaction fee must not be zero.
            // this will only work as long as the transaction list is ordered.
            if ((!_previousTransactions.TryGetValue(command.Transaction.MerchantName.Value, out var previousTransaction) 
                || !command.Transaction.HappenedOnSameMonth(previousTransaction))
                && finalFee.Value > 0)
            {
                finalFee = new Fee(finalFee.Value + _invoiceFixedFee.Value);
            }
            
            _previousTransactions[command.Transaction.MerchantName.Value] = command.Transaction;

            return finalFee;
        }
    }
}