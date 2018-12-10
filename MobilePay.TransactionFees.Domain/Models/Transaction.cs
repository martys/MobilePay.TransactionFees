using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.Domain.Models
{
    public class Transaction
    {
        public Name MerchantName { get; }
        
        public Date Date { get; }
        
        public Amount Amount { get; }

        public Transaction(Date date, Name merchantName, Amount amount)
        {
            MerchantName = merchantName ?? throw new DomainException("Merchant name cannot be null");
            Date = date ?? throw new DomainException("Date cannot be null");
            Amount = amount ?? throw new DomainException("Amount cannot be null");
        }

        public Fee CalculateTransactionPercentageFee(Percentage feePercentage)
        {
            if (feePercentage == null)
            {
                throw new DomainException($"Fee percentage cannot be null");
            }
            
            return new Fee(Amount.Value * feePercentage.Value / 100);
        }

        public bool HappenedOnSameMonth(Transaction other)
        {
            return Date.MonthsMatch(other.Date);
        }
    } 
}