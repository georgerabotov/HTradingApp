using System;
using HTradingApp.Api.ControllerModels;
using HTradingApp.Api.Core;
using HTradingApp.Api.Requests;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BonusPointsEndpoints : ApiControllerBase
	{
		private readonly IAccounts _accountService;
		

        public BonusPointsEndpoints(IMediator mediator, IAccounts accountService)
            : base(mediator)
        {
            _accountService = accountService;
        }

		[HttpGet("{accountId}")]
		public async Task<IActionResult> GetAccountBonusPoints(int accountId)
		{
			return await Ok(new GetBonusPointRequest(accountId));
		}

		[HttpPost("{accountId}")]
		public async Task<IActionResult> AddAccountBonusPoints(int accountId, DateTime fromDateTime, DateTime toDateTime)
		{
            return await Ok(new AddBonusPointRequest(accountId, fromDateTime, toDateTime));
		}

		[HttpPost("{accountId}/credit")]
		public async Task<IActionResult> AddAccountCredit(int accountId)
		{
			return await Ok(new AddCreditRequest(accountId));
        }

		[HttpPost("accounts/credit")]
		public async Task<IActionResult> AddAccountsCredit()
		{
			List<Account> accounts = _accountService.GetAccountsList();
			accounts.ForEach(async x => await Ok(new AddCreditRequest(x.Id)));
			return Created("", "");
        }
	}
}