using System.Collections.Generic;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.AcceptanceTests
{
    // As a MobilePay accountant I want merchants to be charged Transaction Percentage Fee (1% of transaction amount),
    // so that MobilePay would still be cheapest solution in the market and we could earn enough money to cover
    // our expenses  
    public class MOBILEPAY_2 : TransactionFeeCalculatorAcceptanceTest
    {
        protected override ICommandHandler<CalculateFee, Fee> CommandHandler { get; }
        protected override string SourceFilePath => "transactions2.txt";

        public MOBILEPAY_2()
        {
            CommandHandler = new CalculateFeeHandler(new Percentage(1));
        }

        [Fact]
        public void Test()
        {
            // Given that 120 DKK transaction is made to CIRCLE_K     on 2018-09-02                                  
            // And        200 DKK transaction is made to TELIA        on 2018-09-04                                      
            // And        300 DKK transaction is made to CIRCLE_K     on 2018-10-22                                      
            // And        150 DKK transaction is made to CIRCLE_K     on 2018-10-29    
            MakeTransaction(120, "CIRCLE_K", "2018-09-02");
            MakeTransaction(200, "TELIA", "2018-09-04");
            MakeTransaction(300, "CIRCLE_K", "2018-10-22");
            MakeTransaction(150, "CIRCLE_K", "2018-10-29");
            
            //When fees calculation app is executed 
            ExecuteFeeCalculationApp();
            
            //Then the output is:                                                                                       
            //2018-09-02 CIRCLE_K 1.20                                                                        
            //2018-09-04 TELIA    2.00                                                                           
            //2018-10-22 CIRCLE_K 3.00                                                                           
            //2018-10-29 CIRCLE_K 1.50
            TheOutputIs(new List<string>
            {
                "2018-09-02 CIRCLE_K 1.20",
                "2018-09-04 TELIA 2.00",
                "2018-10-22 CIRCLE_K 3.00",
                "2018-10-29 CIRCLE_K 1.50"
            });
        }
    }
}