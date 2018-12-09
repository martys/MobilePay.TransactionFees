using MobilePay.TransactionFees.Domain.Exceptions;

namespace MobilePay.TransactionFees.Domain.ValueObjects
{
    public class Amount
    {
        public double Value { get; }

        public Amount(double value)
        {
            if (value <= 0)
            {
                throw new DomainException($"Invalid amount {value}. Must be a positive number");
            }
            
            Value = value;
        }
    }
}