using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.Repositories;
using MobilePay.TransactionFees.Domain.ValueObjects;
using MobilePay.TransactionFees.InMemoryStorage;
using MobilePay.TransactionFees.Program;
using Xunit;

namespace MobilePay.TransactionFees.AcceptanceTests
{
    // We implement IDisposable to workaround xUnit not having TearDown mechanism
    public abstract class TransactionFeeCalculatorAcceptanceTest : IDisposable
    {
        protected abstract ICommandHandler<CalculateFee, Fee> CommandHandler { get; }
        protected abstract string SourceFilePath { get; }
        protected IMerchantRepository MerchantRepository { get; }
        protected IOutputSettings OutputSettings { get; }
        protected StringBuilder Output { get; }

        protected TransactionFeeCalculatorAcceptanceTest()
        {
            MerchantRepository = new InMemoryMerchantRepository();
            MerchantRepository.Add(new Merchant(new Name("TELIA"), new Percentage(10)));
            MerchantRepository.Add(new Merchant(new Name("CIRCLE_K"), new Percentage(20)));
            MerchantRepository.Add(new Merchant(new Name("7-ELEVEN"), new Percentage(0)));
            MerchantRepository.Add(new Merchant(new Name("NETTO"), new Percentage(0)));
            Output = new StringBuilder();
            OutputSettings = new AcceptanceTestOutputSettings(WriteToStringBuilder);
        }

        protected void MakeTransaction(double amount, string merchantName, string date)
        {
            using (var writer = File.AppendText(SourceFilePath))
            {
                writer.WriteLine($"{date} {merchantName} {amount}");
            }

            var text = File.ReadAllText(SourceFilePath);
        }

        protected void ExecuteFeeCalculationApp()
        {
            var feeCalculationApp = new FeeCalculationApp(CommandHandler, OutputSettings);
            feeCalculationApp.CalculateTransactionFees(SourceFilePath);
        }

        protected void TheOutputIs(IEnumerable<string> expectedOutput)
        {
            var actualOutput = Output.ToString().Split(Environment.NewLine).ToList();
            // There will always be an extra new line symbol, and thus an extra empty element. Remove it
            actualOutput.RemoveAt(actualOutput.Count - 1);

            Assert.Equal(expectedOutput.Count(), actualOutput.Count);

            for (var i = 0; i < actualOutput.Count; i++)
            {
                Assert.Equal(expectedOutput.ElementAt(i), actualOutput[i]);
            }

        }
        
        protected void WriteToStringBuilder(string outputLine)
        {
            Output.AppendLine(outputLine);
        }

        // Cleanup the transaction file so we don't leave any trash after the run
        public void Dispose()
        {
            File.Delete(SourceFilePath);
        }

        protected class AcceptanceTestOutputSettings : IOutputSettings
        {
            public AcceptanceTestOutputSettings(Action<string> test)
            {
                WriteToOutput = test;
            }
            
            public Action<string> WriteToOutput { get; private set; }
            public string DateFormatting => "yyyy-MM-dd";
            public string FeeFormatting => "F";
        }
    }
}