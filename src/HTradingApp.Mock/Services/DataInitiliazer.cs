using System;
using Bogus;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Mock.Services
{
	public class DataInitiliazer
	{
		private readonly IMemoryCache _cache;
		public DataInitiliazer(IMemoryCache cache)
		{
            _cache = cache;
        }

        public void GenerateFakeData() => GenerateAccounts();

		private void GenerateAccounts()
		{
            // Make sure our ids are from 1 to 5.
            // Generate Random Names
            List<Account> accounts = new();
            for (int i = 1; i < 6; i++)
            {
                accounts.Add(new Faker<Account>()
                .RuleFor(x => x.Id, i)
                .RuleFor(x => x.Name, f => f.Name.FullName()));
            }
            
            List<Deal> deals = accounts
                .SelectMany(account => GenerateDeals(account.Id))
                .ToList();

            List<BonusPoint> bonusPoints = accounts.Select(account => GenerateBonusPoints(account.Id)).ToList();

            _cache.Set("Accounts", accounts);
            _cache.Set("Deals", deals);
            _cache.Set("BonusPoints", bonusPoints);
            
        }

        // Generating deals only in the last month, since we will run it on a monthly basis
        private List<Deal> GenerateDeals(int accountId)
        {
            return new Faker<Deal>()
                .RuleFor(x => x.AccountId, accountId)
                .RuleFor(x => x.Amount, f => f.Random.Int(0, 1000000))
                .RuleFor(x => x.DealDateTime, f => f.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(-1)))
                .Generate(5);
        }

        private BonusPoint GenerateBonusPoints(int accountId)
        {
            // Adding 10 Bonus points on Account Creation
            return new Faker<BonusPoint>()
                .RuleFor(x => x.AccountId, accountId)
                .RuleFor(x => x.Amount, 10)
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.BonusAdded, DateTime.Now);
        }
	}
}