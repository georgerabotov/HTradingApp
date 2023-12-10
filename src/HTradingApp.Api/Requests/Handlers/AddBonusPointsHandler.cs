using System;
using HTradingApp.Api.Requests.Responses;
using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using MediatR;

namespace HTradingApp.Api.Requests.Handlers
{
	public class AddBonusPointsHandler : IRequestHandler<AddBonusPointRequest, bool>
	{
        private readonly IBonusService _bonusService;
        private readonly IDeals _deals;

        public AddBonusPointsHandler(IBonusService bonusService, IDeals deals)
        {
            _bonusService = bonusService;
            _deals = deals;
        }

        public async Task<bool> Handle(AddBonusPointRequest request, CancellationToken cancellationToken)
        {
            // If more time, I would move the deals at the endpoint level.

            List<Deal> deals = _deals.GetHistoricalDeals(request.AccountId, request.FromDateTime, request.ToDateTime);
            int bonusPoints = _bonusService.CalculateBonusPoints(request.AccountId, deals);
            return _bonusService.AddAccountBonusPoints(request.AccountId, bonusPoints);
        }
    }
}

