using MobilePay.TransactionFees.Domain.Exceptions;

namespace MobilePay.TransactionFees.Domain.ValueObjects
{
    public class Name
    {
        public string Value { get; }

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Name cannot be empty");
            }

            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}