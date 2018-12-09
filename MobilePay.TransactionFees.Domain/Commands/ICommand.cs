using System;

namespace MobilePay.TransactionFees.Domain.Commands
{
    public interface ICommand<TResult>
    {
        Guid CorrelationId { get; }
    }
}