using FluentValidation;
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
                .Must(x => AccountValidityHelper.BeValidAccount(x, _cache))
                .WithMessage("Account does not exist");

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

