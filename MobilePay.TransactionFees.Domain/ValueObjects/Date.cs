using System;

namespace MobilePay.TransactionFees.Domain.ValueObjects
{
    public class Date
    {
        public DateTime Value { get; }

        public Date(DateTime value)
        {
            Value = value;
        }

        public bool MonthsMatch(Date other)
        {
            if (other == null)
            {
                return false;
            }
            
            return Value.Year == other.Value.Year && Value.Month == other.Value.Month;
        }
    }
}