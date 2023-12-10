﻿using System;
using HTradingApp.Domain.Models;

namespace HTradingApp.Domain
{
	public interface IBonusService
	{
		public Task<int> GetAccountBonusPoints(int accountId);
		public Task<bool> AddAccountBonusPoints(int accountId, int bonusPoints);
		public Task<int> CalculateBonusPoints(int accountId, List<Deal> deals);
		public Task<bool> FlushBonusPoints(int accountId);
		public Task<decimal> CalculateAccountCredit(int bonusPoints);
		public Task<bool> IsEligibleForBonusPoints(int accountId, List<Deal> deals);
	}
}