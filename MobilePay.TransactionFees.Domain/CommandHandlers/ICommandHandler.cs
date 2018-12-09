using MobilePay.TransactionFees.Domain.Commands;

namespace MobilePay.TransactionFees.Domain.CommandHandlers
{
    public interface ICommandHandler<in TCommand, out TResult>
        where TCommand : ICommand<TResult>
    {
        TResult Handle(TCommand command);
    }
}