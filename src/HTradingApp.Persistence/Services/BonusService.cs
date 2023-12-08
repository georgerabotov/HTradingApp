using System;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;

namespace HTradingApp.Persistence.Services
{
    public class BonusService : IBonusService
    {
        public Task<int> AddAccountBonusPoints(int accountId, int bonusPoints)
        {
            throw new NotImplementedException();
        }

        public Task<int> CalculateBonusPoints(int accountId, List<Deal> deals)
        {
            throw new NotImplementedException();
        }

        public Task<int> CalculateBonusPointsCredit(int accountId, int bonusPoints)
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
    }
}

