using System;
using HTradingApp.Domain.Models;

namespace HTradingApp.Domain
{
	public interface IAccounts
	{
        List<Account> GetAccountsList();
    }
}