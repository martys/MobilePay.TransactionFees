using System;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.Domain.Commands
{
    public class CalculateFee : ICommand<Fee>
    {
        public Guid CorrelationId { get; }
        public Transaction Transaction { get; }

        public CalculateFee(Guid correlationId, Transaction transaction)
        {
            CorrelationId = correlationId;
            Transaction = transaction;
        }
    }
}