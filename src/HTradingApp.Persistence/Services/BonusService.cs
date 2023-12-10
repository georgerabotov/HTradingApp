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
            List<BonusPoint>? bonusPointList = _cache.Get("BonusPoints") as List<BonusPoint>;

            if (bonusPointList == null)
            {
                return false;
            }

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

            _cache.Set("BonusPoints", bonusPointList);

            List<BonusPoint>? cacheUpdated = _cache.Get("BonusPoints") as List<BonusPoint>;
            return cacheUpdated?.Count == bonusPointList.Count;
        }

        public decimal CalculateAccountCredit(int bonusPoints)
        {
            decimal bonusMultiplier = 12.5M;

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
            {
                bonusPoints += 10;
            }

            // Defining thresholds instead of if statements
            Dictionary<decimal, int> bonusThresholds = new Dictionary<decimal, int>
            {
                { 10000000, 45 },
                { 1000000, 30 },
                { 100000, 20 },
                { 10000, 10 },
                { 1000, 5 }
            };

            foreach (var threshold in bonusThresholds)
            {
                if (dealsSum >= threshold.Key)
                {
                    bonusPoints += threshold.Value;
                }
            }

            return bonusPoints;
        }

        public bool FlushBonusPoints(int accountId)
        {
            if (!(_cache.Get("BonusPoints") is List<BonusPoint> bonusPoints))
            {
                return false;
            }

            foreach (var bonusPoint in bonusPoints
                .Where(x => !x.ConvertedToCredit && x.AccountId == accountId))
            {
                bonusPoint.ConvertedToCredit = true;
            }

            _cache.Set("BonusPoints", bonusPoints);

            return !bonusPoints.Any(x => !x.ConvertedToCredit && x.AccountId == accountId);
        }

        public int GetAccountBonusPoints(int accountId)
        {
            var bonusPoints = _cache.Get("BonusPoints") as List<BonusPoint>;
            if (bonusPoints == null)
            {
                return 0;
            }

            return bonusPoints
                .Where(x => x.AccountId == accountId && !x.ConvertedToCredit)
                .Sum(bp => bp.Amount);
        }

        public bool IsEligibleForBonusPoints(int accountId, List<Deal> deals)
        {
            // The trader needs to do at least 3 deals in order to get bonus points.
            return deals.Count() > 3;
        }
    }
}

