using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Exceptions;
using MobilePay.TransactionFees.Domain.Repositories;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.CommandHandlers
{
    public class CalculateFeeWithDiscountHandler: ICommandHandler<CalculateFee, Fee>
    {
        private readonly ICommandHandler<CalculateFee, Fee> _calculateFeeHandler;
        private readonly IMerchantRepository _merchantRepository;
        
        public CalculateFeeWithDiscountHandler(ICommandHandler<CalculateFee, Fee> calculateFeeHandler,
            IMerchantRepository merchantRepository)
        {
            _calculateFeeHandler = calculateFeeHandler;
            _merchantRepository = merchantRepository;
        }
        
        public Fee Handle(CalculateFee command)
        {
            var merchant = _merchantRepository.Get(command.Transaction.MerchantName);
            if (merchant == null)
            {
                throw new DomainException($"Merchant {command.Transaction.MerchantName} not found");
            }

            var standardFee = _calculateFeeHandler.Handle(command);

            return merchant.ApplyTransactionPercentageFeeDiscount(standardFee);
        }
    }
}