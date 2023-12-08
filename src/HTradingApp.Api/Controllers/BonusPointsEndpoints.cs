using System;
using HTradingApp.Domain;
using Microsoft.AspNetCore.Mvc;


namespace HTradingApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BonusPointsEndpoints : ControllerBase
	{
		private readonly IBonusService _bonusService;
		private readonly IDeals _dealService;
		private readonly ICreditOperations _creditService;
		private readonly IAccounts _accountService;

        public BonusPointsEndpoints(IBonusService bonusService, IDeals dealService, ICreditOperations creditService, IAccounts accountService)
		{
            _bonusService = bonusService;
            _dealService = dealService;
            _creditService = creditService;
            _accountService = accountService;
        }

		[HttpGet("{accointId}")]
		public async Task<IActionResult> GetAccountBonusPoints(int accountId)
		{
			return Ok(await _bonusService.GetAccountBonusPoints(accountId));
		}

		[HttpPost("{accountId}")]
		public async Task<IActionResult> AddAccountBonusPoints(int accountId, DateTime fromDateTime, DateTime toDateTime)
		{
			var account = _accountService.GetAccountsList();
			var deals = _dealService.GetHistoricalDeals(accountId, fromDateTime, toDateTime);

            int bonusPoints = await _bonusService.CalculateBonusPoints(accountId, deals);

			return Ok(await _bonusService.AddAccountBonusPoints(accountId, bonusPoints));
		}

		[HttpPost("{accountId}/credit")]
		public async Task<IActionResult> AddAccountCredit(int accountId)
		{
			int bonusPoints = await _bonusService.GetAccountBonusPoints(accountId);
			if (bonusPoints == 0)
			{
				return NoContent();
			}
			int credit = await _bonusService.CalculateBonusPointsCredit(accountId, bonusPoints);

			bool isSuccessful = _creditService.CreateCreditOperation(accountId, credit);
			if (!isSuccessful)
			{
				return BadRequest("Failed to add credit to account");
			}

            await _bonusService.FlushBonusPoints(accountId);
            return Ok($"Successfully Credited {accountId} with £{credit}");
        }

		[HttpPost("accounts/credit")]
		public async Task<IActionResult> AddAccountsCredit()
		{
			return Ok();
		}
	}
}