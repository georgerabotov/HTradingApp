using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace HTradingApp.Api.Requests.Validators
{
    public class AddCreditValidator : AbstractValidator<AddCreditRequest>
    {
        private readonly IMemoryCache _cache;

		public AddCreditValidator(IMemoryCache cache)
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

