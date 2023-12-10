using HTradingApp.Domain;
using HTradingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HTradingApp.Api.Requests.Handlers
{
    public class AddBonusPointsHandler : IRequestHandler<AddBonusPointRequest, IActionResult>
	{
        private readonly IBonusService _bonusService;
        private readonly IDeals _deals;

        public AddBonusPointsHandler(IBonusService bonusService, IDeals deals)
        {
            _bonusService = bonusService;
            _deals = deals;
        }

        public async Task<IActionResult> Handle(AddBonusPointRequest request, CancellationToken cancellationToken)
        {
            List<Deal> deals = _deals.GetHistoricalDeals(request.AccountId, request.FromDateTime, request.ToDateTime);
            int bonusPoints = _bonusService.CalculateBonusPoints(request.AccountId, deals);
            bool successful =  _bonusService.AddAccountBonusPoints(request.AccountId, bonusPoints);
            return successful == true
                ? new CreatedResult("The account had it's bonus points calculated and added", bonusPoints)
                : new BadRequestObjectResult($"Failed to add bonus points: {bonusPoints}");
        }
    }
}

