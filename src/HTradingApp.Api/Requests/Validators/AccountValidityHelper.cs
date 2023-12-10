using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Api.Requests.Validators
{
    public static class AccountValidityHelper
	{
        public static bool BeValidAccount(int accountId, IMemoryCache cache)
        {
            var accounts = cache.Get("Accounts") as List<Account>;
            return accounts != null && accounts.Any(y => y.Id == accountId);
        }
    }
}

