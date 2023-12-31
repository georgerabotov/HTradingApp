﻿using HTradingApp.Domain;
using HTradingApp.Domain.Models;

namespace HTradingApp.Mock.Services
{
    public class CreditService : ICreditOperations
    {
        private readonly IAccounts _accountService;

        public CreditService(IAccounts accountService)
        {
            _accountService = accountService;
        }
        public bool CreateCreditOperation(int accountId, decimal amount)
        {
            List<Account> accounts = _accountService.GetAccountsList();
            if (!accounts.Any(x => x.Id == accountId))
            {
                return false;
            }
            // Here it would probably call the API to confirm the successful addition of a credit.
            Console.WriteLine($"Created Credit operation of {amount} for accountId {accountId}");
            return true;
        }
    }
}

