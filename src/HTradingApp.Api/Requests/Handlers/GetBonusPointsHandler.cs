using HTradingApp.Api.ControllerModels;
using HTradingApp.Api.Requests.Responses;
using HTradingApp.Domain;
using MediatR;

namespace HTradingApp.Api.Requests.Handlers
{
    public class GetBonusPointsHandler : IRequestHandler<GetBonusPointRequest, BonusPointsResponse>
	{
        private readonly IBonusService _bonusService;

		public GetBonusPointsHandler(IBonusService bonusService)
		{
            _bonusService = bonusService;
        }

        public async Task<BonusPointsResponse> Handle(GetBonusPointRequest request, CancellationToken cancellationToken)
        {
            int bonusPoints = _bonusService.GetAccountBonusPoints(request.AccountId);
            return new BonusPointsResponse(request.AccountId, bonusPoints);
        }
    }
}

