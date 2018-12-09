using MobilePay.TransactionFees.Domain.Exceptions;

namespace MobilePay.TransactionFees.Domain.ValueObjects
{
    public class Percentage
    {
        public double Value { get; }

        public Percentage(double value)
        {
            if (value < 0 || value > 100)
            {
                throw new DomainException("Percentage must fall between 0 and a 100");
            }

            Value = value;
        }
    }
}