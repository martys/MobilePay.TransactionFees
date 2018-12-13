using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.Domain.Models
{
    public class Merchant
    {
        public Name Name { get; }
        public Percentage TransactionPercentageFeeDiscount { get; }

        public Merchant(Name name, Percentage transactionPercentageFeeDiscount)
        {
            Name = name ?? throw new DomainException("Name cannot be null");
            TransactionPercentageFeeDiscount = transactionPercentageFeeDiscount ?? 
                throw new DomainException("Transaction percentage fee discount cannot be null");
        }

        public Fee ApplyTransactionPercentageFeeDiscount (Fee fee)
        {
            if (fee == null)
            {
                throw new DomainException("Fee cannot be null");
            }
            
            var discount = fee.Value * TransactionPercentageFeeDiscount.Value / 100;
            return new Fee(fee.Value - discount);
        }
    }
}