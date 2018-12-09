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
            MerchantName = merchantName;
            Date = date;
            Amount = amount;
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