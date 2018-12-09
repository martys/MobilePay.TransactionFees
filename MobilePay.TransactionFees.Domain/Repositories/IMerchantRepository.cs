using System.Collections.Generic;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.Domain.Repositories
{
    public interface IMerchantRepository
    {
        IEnumerable<Merchant> GetAll();
        Merchant Get(Name name);
        void Add(Merchant merchant);
        void Delete(Merchant merchant);
    }
}