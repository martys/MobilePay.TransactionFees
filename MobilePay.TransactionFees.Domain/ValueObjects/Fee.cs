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
        
        //TODO - do we need to always take floor instead of rounding, so we don't 'overcharge' the client?
        public override string ToString()
        {
            return Value.ToString("F", CultureInfo.InvariantCulture);
        }
    }
}