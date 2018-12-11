using System;
using System.IO;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;
using MobilePay.TransactionFees.InMemoryStorage;

namespace MobilePay.TransactionFees.Program
{
    class Program
    {
        // These should be configurable via .NET Core Configuration API, but it requires a nuget package
        public const double TransactionPercentageFee = 1;
        public const double InvoiceFixedFee = 29;
        public const string InputFilePath = "transactions.txt";
        
        static void Main(string[] args)
        {
            var merchantRepository = new InMemoryMerchantRepository();
            merchantRepository.Add(new Merchant(new Name("7-ELEVEN"), new Percentage(0)));
            merchantRepository.Add(new Merchant(new Name("NETTO"), new Percentage(0)));
            merchantRepository.Add(new Merchant(new Name("TELIA"), new Percentage(10)));
            merchantRepository.Add(new Merchant(new Name("CIRCLE_K"), new Percentage(20)));
            
            
            var calculateFeeHandler = new CalculateFeeHandler(new Percentage(TransactionPercentageFee));
            var calculateFeeWithDiscountHandler = new CalculateFeeWithDiscountHandler(calculateFeeHandler, 
                merchantRepository);
            var calculateWithInvoiceFeeHandler = new CalculateFeeWithInvoiceFeeHandler(calculateFeeWithDiscountHandler, 
                new Fee(InvoiceFixedFee));

            var transactionFeeCalculator = new FeeCalculationApp(calculateWithInvoiceFeeHandler);
            transactionFeeCalculator.CalculateTransactionFees(InputFilePath);
        }
    }
}