using System;
using HTradingApp.Domain.Models;

namespace HTradingApp.Domain
{
	public interface IDeals
	{
        List<Deal> GetHistoricalDeals(int accountId, DateTime fromDateTime, DateTime toDateTime);
    }
}

