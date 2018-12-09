using System;
using System.Collections.Generic;
using System.Linq;
using MobilePay.TransactionFees.Domain.Models;
using MobilePay.TransactionFees.Domain.Repositories;
using MobilePay.TransactionFees.Domain.ValueObjects;

namespace MobilePay.TransactionFees.InMemoryStorage
{
    public class InMemoryMerchantRepository : IMerchantRepository
    {
        private readonly ICollection<Merchant> _merchants;

        public InMemoryMerchantRepository()
        {
            _merchants = new List<Merchant>();
        }
        
        public IEnumerable<Merchant> GetAll()
        {
            return _merchants;
        }

        public Merchant Get(Name name)
        {
            return _merchants.SingleOrDefault(x => x.Name.Value == name.Value);
        }

        public void Add(Merchant merchant)
        {
            _merchants.Add(merchant);
        }

        public void Delete(Merchant merchant)
        {
            _merchants.Remove(merchant);
        }
    }
}