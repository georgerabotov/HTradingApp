using System;
using FluentValidation;
using HTradingApp.Api.ControllerModels;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Api.Requests.Validators
{
	public class GetBonusPointValidator : AbstractValidator<GetBonusPointRequest>
	{
		private readonly IMemoryCache _cache;

		public GetBonusPointValidator(IMemoryCache cache)
		{
            _cache = cache;

			RuleFor(x => x.AccountId)
				.NotNull()
				.NotEmpty()
				.Must(x =>
				{
					var accounts = (List<Account>)_cache.Get("Accounts");
					return accounts.Any(y => y.Id == x);
				}).WithMessage("Account does not exist");
        }
	}
}

