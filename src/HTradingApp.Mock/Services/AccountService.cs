using System;
using Bogus;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Mock.Services
{
	public class AccountService : IAccounts
	{
        private readonly IMemoryCache _cache;
        public AccountService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public List<Account> GetAccountsList()
        {
            return (List<Account>) _cache.Get("Accounts");
        }
    }
}