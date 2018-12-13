using System.Collections.Generic;
using MobilePay.TransactionFees.CommandHandlers;
using MobilePay.TransactionFees.Domain.CommandHandlers;
using MobilePay.TransactionFees.Domain.Commands;
using MobilePay.TransactionFees.Domain.ValueObjects;
using Xunit;

namespace MobilePay.TransactionFees.AcceptanceTests
{
    public class MOBILEPAY_4 : TransactionFeeCalculatorAcceptanceTest
    {
        protected override ICommandHandler<CalculateFee, Fee> CommandHandler { get; }
        protected override string SourceFilePath => "transactions4.txt";

        public MOBILEPAY_4()
        {
            CommandHandler = new CalculateFeeWithDiscountHandler(
                new CalculateFeeHandler(new Percentage(1)), MerchantRepository);
        }

        [Fact]
        public void Test()
        {
            // Given that 120 DKK transaction is made to CIRCLE_K on 2018-09-02                                             
            // And        200 DKK transaction is made to CIRCLE_K on 2018-09-04                                             
            // And        300 DKK transaction is made to CIRCLE_K on 2018-10-22                                             
            // And        150 DKK transaction is made to CIRCLE_K on 2018-10-29    
            MakeTransaction(120, "CIRCLE_K", "2018-09-02");
            MakeTransaction(200, "CIRCLE_K", "2018-09-04");
            MakeTransaction(300, "CIRCLE_K", "2018-10-22");
            MakeTransaction(150, "CIRCLE_K", "2018-10-29");

            //When fees calculation app is executed 
            ExecuteFeeCalculationApp();

            //Then the output is:                                                                                       
            //2018-09-02 CIRCLE_K 0.96                                                                        
            //2018-09-04 CIRCLE_K 1.60                                                                           
            //2018-10-22 CIRCLE_K 2.40                                                                           
            //2018-10-29 CIRCLE_K 1.20   
            TheOutputIs(new List<string>
            {
                "2018-09-02 CIRCLE_K 0.96",
                "2018-09-04 CIRCLE_K 1.60",
                "2018-10-22 CIRCLE_K 2.40",
                "2018-10-29 CIRCLE_K 1.20"
            });
        }
    }
}