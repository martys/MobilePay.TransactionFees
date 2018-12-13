using System.Collections.Generic;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.AcceptanceTests
{
    // As a MobilePay accountant I want to charge merchants Invoice Fixed Fee (29 DKK) every month, so that we could  
    // cover our expenses for sending physical invoices to merchants    
    public class MOBILEPAY_5 : TransactionFeeCalculatorAcceptanceTest
    {
        protected override ICommandHandler<CalculateFee, Fee> CommandHandler { get; }
        protected override string SourceFilePath => "transactions5.txt";

        public MOBILEPAY_5()
        {
            var calculateFeeHandler = new CalculateFeeHandler(new Percentage(1));
            var calculateWithDiscountHandler = new CalculateFeeWithDiscountHandler(calculateFeeHandler, MerchantRepository);
            CommandHandler = new CalculateFeeWithInvoiceFeeHandler(calculateWithDiscountHandler, new Fee(29));
        }

        [Fact]
        public void Test()
        {
            // Given that 120 DKK transaction is made to 7-ELEVEN on 2018-09-02                                             
            // And        200 DKK transaction is made to NETTO on 2018-09-04                                             
            // And        300 DKK transaction is made to 7-ELEVEN on 2018-10-22                                             
            // And        150 DKK transaction is made to 7-ELEVEN on 2018-10-29    
            MakeTransaction(120, "7-ELEVEN", "2018-09-02");
            MakeTransaction(200, "NETTO", "2018-09-04");
            MakeTransaction(300, "7-ELEVEN", "2018-10-22");
            MakeTransaction(150, "7-ELEVEN", "2018-10-29");

            //When fees calculation app is executed 
            ExecuteFeeCalculationApp();

            //Then the output is:                                                                                       
            //2018-09-02 7-ELEVEN 30.20                                                                        
            //2018-09-04 NETTO 31.00                                                                           
            //2018-10-22 7-ELEVEN 32.00                                                                           
            //2018-10-29 7-ELEVEN 1.50   
            TheOutputIs(new List<string>
            {
                "2018-09-02 7-ELEVEN 30.20",
                "2018-09-04 NETTO 31.00",
                "2018-10-22 7-ELEVEN 32.00",
                "2018-10-29 7-ELEVEN 1.50"
            });
        }
    }
}