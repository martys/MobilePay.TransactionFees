using System;
using System.IO;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.Program
{
    public class FeeCalculationApp
    {
        private readonly ICommandHandler<CalculateFee, Fee> _calculateFeeHandler;

        public FeeCalculationApp(ICommandHandler<CalculateFee, Fee> calculateFeeHandler)
        {
            _calculateFeeHandler = calculateFeeHandler;
        }

        public void CalculateTransactionFees(string sourceFilePath)
        {
            var line = string.Empty;
            using (var fileReader = new StreamReader(sourceFilePath))
            {
                while((line = fileReader.ReadLine()) != null)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var transactionData = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            var transaction = new Transaction(new Date(DateTime.Parse(transactionData[0])),
                                new Name(transactionData[1]),
                                new Amount(double.Parse(transactionData[2])));
                            var command = new CalculateFee(Guid.NewGuid(), transaction);
                            var transactionFee = _calculateFeeHandler.Handle(command);
                            Console.WriteLine($"{transaction.Date} {transaction.MerchantName} {transactionFee}");
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }   
            }
        }
    }
}