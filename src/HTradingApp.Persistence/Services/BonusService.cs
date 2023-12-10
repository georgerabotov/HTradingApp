using System;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Persistence.Services
{
    public class BonusService : IBonusService
    {
        private readonly IMemoryCache _cache;

        public BonusService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<bool> AddAccountBonusPoints(int accountId, int bonusPoints)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> CalculateAccountCredit(int bonusPoints)
        {
            throw new NotImplementedException();
        }

        public Task<int> CalculateBonusPoints(int accountId, List<Deal> deals)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FlushBonusPoints(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccountBonusPoints(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEligibleForBonusPoints(int accountId, List<Deal> deals)
        {
            throw new NotImplementedException();
        }
    }
}

