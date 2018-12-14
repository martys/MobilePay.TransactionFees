using System;

namespace MobilePay.TransactionFees.Program
{
    public class OutputSettings : IOutputSettings
    {
        public Action<string> WriteToOutput => Console.WriteLine;
        public string DateFormatting => "yyyy-MM-dd";
        public string FeeFormatting => "F";
    }
}