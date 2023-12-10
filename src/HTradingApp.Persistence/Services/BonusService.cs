using System;
using System.Collections.Generic;
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

        public bool AddAccountBonusPoints(int accountId, int bonusPoints)
        {
            List<BonusPoint> bonusPointList = (List<BonusPoint>)_cache.Get("BonusPoints");

            if (bonusPointList.Any(x => x.AccountId == accountId))
            {
                return false;
            }

            var newBonusPoint = new BonusPoint
            {
                AccountId = accountId,
                Amount = bonusPoints,
                Id = Guid.NewGuid(),
                BonusAdded = DateTime.Now,
                ConvertedToCredit = false
            };

            bonusPointList.Add(newBonusPoint);

            _cache.Set<List<BonusPoint>>("BonusPoints", bonusPointList);

            List<BonusPoint> cacheUpdated = (List<BonusPoint>)_cache.Get("BonusPoints");
            bool isAdded = cacheUpdated.Count() == bonusPointList.Count();

            return isAdded;
        }

        public decimal CalculateAccountCredit(int bonusPoints)
        {
            const decimal bonusMultiplier = 12.5M;

            return Math.Round(bonusPoints * bonusMultiplier, 2);
        }

        public int CalculateBonusPoints(int accountId, List<Deal> deals)
        {
            if (!IsEligibleForBonusPoints(accountId, deals))
            {
                return 0;
            }

            int bonusPoints = 0;
            decimal dealsSum = deals.Select(x => x.Amount).Sum();
            if (deals.Count >= 10)
                bonusPoints += 10;

            if (dealsSum >= 1000)
                bonusPoints += 5;

            if (dealsSum >= 10000)
                bonusPoints += 10;

            if (dealsSum >= 100000)
                bonusPoints += 20;

            if (dealsSum >= 1000000)
                bonusPoints += 30;

            if (dealsSum >= 10000000)
                bonusPoints += 45;

            return bonusPoints;
        }

        public bool FlushBonusPoints(int accountId)
        {
            List<BonusPoint> bonusPoints = (List<BonusPoint>)_cache.Get("BonusPoints");
            foreach (BonusPoint bonusPoint in bonusPoints)
            {
                if (bonusPoint.ConvertedToCredit == false && bonusPoint.AccountId == accountId)
                {
                    bonusPoint.ConvertedToCredit = true;
                }
            }

            _cache.Set("BonusPoints", bonusPoints);

            List<BonusPoint> updatedList = (List<BonusPoint>)_cache.Get("BonusPoints");
            return !updatedList.Any(x => x.ConvertedToCredit == false && x.AccountId == accountId);
        }

        public int GetAccountBonusPoints(int accountId)
        {
            var bonusPoints = (List<BonusPoint>)_cache.Get("BonusPoints");
            var bonusPointsForAccount = bonusPoints
                .Where(x => x.AccountId == accountId && x.ConvertedToCredit == false)
                .Select(y => y.Amount);

            return bonusPointsForAccount.Sum();
        }

        public bool IsEligibleForBonusPoints(int accountId, List<Deal> deals)
        {
            // The trader needs to do at least 3 deals in order to get bonus points.
            return deals.Count() > 3;
        }
    }
}

