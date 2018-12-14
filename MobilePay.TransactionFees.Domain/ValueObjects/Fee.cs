using System.Globalization;
using MobilePay.TransactionFees.Domain.Exceptions;

namespace MobilePay.TransactionFees.Domain.ValueObjects
{
    public class Fee
    {
        public double Value { get; }

        public Fee(double value)
        {
            if (value < 0)
            {
                throw new DomainException("Transaction fee cannot be negative");
            }
            Value = value;
        }
    }
}