using System;
using HTradingApp.Api.Requests.Responses;
using HTradingApp.Domain;
using MediatR;

namespace HTradingApp.Api.Requests.Handlers
{
	public class AddBonusPointsHandler : IRequestHandler<AddBonusPointRequest, bool>
	{
        private readonly IBonusService _bonusService;
        public AddBonusPointsHandler(IBonusService bonusService)
        {
            _bonusService = bonusService;
        }

        public async Task<bool> Handle(AddBonusPointRequest request, CancellationToken cancellationToken)
        {
            return await _bonusService.AddAccountBonusPoints(request.AccountId, request.Amount);
        }
    }
}

