using System;
using System.Collections.Generic;
using Bogus;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Mock.Services
{
    public class DealService : IDeals
    {
        private readonly IMemoryCache _cache;
        public DealService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public List<Deal> GetHistoricalDeals(int accountId, DateTime fromDateTime, DateTime toDateTime)
        {
            var deals = (List<Deal>)_cache.Get("Deals");
            return deals;
        }
    }
}