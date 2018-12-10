using System;
using MobilePay.TransactionFees.Domain.Exceptions;
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
            if (correlationId == default(Guid))
            {
                throw new DomainException("Correlation id cannot be empty");
            }
            
            CorrelationId = correlationId;
            Transaction = transaction ?? throw new DomainException("Transaction cannot be null");
        }
    }
}