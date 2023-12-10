using HTradingApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HTradingApp.Api.Requests.Handlers
{
    public class AddCreditHandler : IRequestHandler<AddCreditRequest, IActionResult>
	{
        private readonly IBonusService _bonusService;
        private readonly ICreditOperations _creditOperations;
		public AddCreditHandler(IBonusService bonusService, ICreditOperations creditOperations)
		{
            _bonusService = bonusService;
            _creditOperations = creditOperations;
        }

        public async Task<IActionResult> Handle(AddCreditRequest request, CancellationToken cancellationToken)
        {
            int bonusPoints =  _bonusService.GetAccountBonusPoints(request.AccountId);
            decimal creditAmount = _bonusService.CalculateAccountCredit(bonusPoints);
            bool isCredited = _creditOperations.CreateCreditOperation(request.AccountId, creditAmount);
            if (isCredited)
            {
                _ = _bonusService.FlushBonusPoints(request.AccountId);
                return new CreatedResult("Bonus points were successfully credited", bonusPoints);
            }
            return new BadRequestObjectResult($"Failed to add bonus points: {bonusPoints}");
        }
    }
}