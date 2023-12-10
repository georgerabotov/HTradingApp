using System;
using HTradingApp.Domain;
using MediatR;

namespace HTradingApp.Api.Requests.Handlers
{
	public class AddCreditHandler : IRequestHandler<AddCreditRequest, bool>
	{
        private readonly IBonusService _bonusService;
        private readonly ICreditOperations _creditOperations;
		public AddCreditHandler(IBonusService bonusService, ICreditOperations creditOperations)
		{
            _bonusService = bonusService;
            _creditOperations = creditOperations;
        }

        public async Task<bool> Handle(AddCreditRequest request, CancellationToken cancellationToken)
        {
            int bonusPoints =  _bonusService.GetAccountBonusPoints(request.AccountId);
            decimal creditAmount = _bonusService.CalculateAccountCredit(bonusPoints);
            bool isCredited = _creditOperations.CreateCreditOperation(request.AccountId, creditAmount);
            if (isCredited)
            {
                _ = _bonusService.FlushBonusPoints(request.AccountId);
                return true;
            }
            return false;
        }
    }
}