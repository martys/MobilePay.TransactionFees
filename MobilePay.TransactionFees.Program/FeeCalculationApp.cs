using System;
using System.Globalization;
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
        private readonly IOutputSettings _outputSettings;

        public FeeCalculationApp(ICommandHandler<CalculateFee, Fee> calculateFeeHandler, IOutputSettings outputSettingss)
        {
            _calculateFeeHandler = calculateFeeHandler;
            _outputSettings = outputSettingss;
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

                            var formattedDate = transaction.Date.Value.ToString(_outputSettings.DateFormatting,
                                CultureInfo.InvariantCulture);
                            var formattedFee = transactionFee.Value.ToString(_outputSettings.FeeFormatting,
                                CultureInfo.InvariantCulture);
                            var formattedMerchantName = transaction.MerchantName.Value;
                            _outputSettings.WriteToOutput(
                                $"{formattedDate} {formattedMerchantName} {formattedFee}");
                        }
                        else
                        {
                            _outputSettings.WriteToOutput(Environment.NewLine);
                        }
                    }
                    catch (Exception e)
                    {
                        _outputSettings.WriteToOutput(e.Message);
                    }
                }   
            }
        }
    }
}