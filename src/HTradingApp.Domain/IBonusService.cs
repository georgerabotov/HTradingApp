using System;
using HTradingApp.Domain.Models;

namespace HTradingApp.Domain
{
	public interface IBonusService
	{
		public int GetAccountBonusPoints(int accountId);
		public bool AddAccountBonusPoints(int accountId, int bonusPoints);
		public int CalculateBonusPoints(int accountId, List<Deal> deals);
		public bool FlushBonusPoints(int accountId);
		public decimal CalculateAccountCredit(int bonusPoints);
		public bool IsEligibleForBonusPoints(int accountId, List<Deal> deals);
	}
}