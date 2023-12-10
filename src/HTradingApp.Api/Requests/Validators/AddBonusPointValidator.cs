using System;
using FluentValidation;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Api.Requests.Validators
{
	public class AddBonusPointValidator : AbstractValidator<AddBonusPointRequest>
	{
        private readonly IMemoryCache _cache;

		public AddBonusPointValidator(IMemoryCache cache)
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

            // Company opens in 1990
            RuleFor(x => x.FromDateTime)
                .NotNull()
                .NotEmpty()
                .GreaterThan(new DateTime(1990, 1, 1));

            RuleFor(x => x.ToDateTime)
                .NotNull()
                .NotEmpty()
                .GreaterThan(new DateTime(1990, 1, 1));
        }
	}
}

