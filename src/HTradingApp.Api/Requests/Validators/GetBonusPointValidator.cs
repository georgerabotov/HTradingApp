using FluentValidation;
using HTradingApp.Api.ControllerModels;
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
                .Must(x => AccountValidityHelper.BeValidAccount(x, _cache))
                .WithMessage("Account does not exist");
        }
	}
}

