using System;

namespace MobilePay.TransactionFees.Program
{
    public interface IOutputSettings
    {
        Action<string> WriteToOutput { get; }
        string DateFormatting { get; }
        string FeeFormatting { get; }
    }
}