using System;

namespace MobilePay.TransactionFees.Domain.Exceptions
{
    public class DomainObjectNotFoundException : DomainException
    {
        public DomainObjectNotFoundException()
        {
        }

        public DomainObjectNotFoundException(string message)
            : base(message)
        {
        }

        public DomainObjectNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}