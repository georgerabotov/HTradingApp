using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.ControllerModels
{
	public class GetBonusPointRequest : IRequest<BonusPointsResponse>
	{
		public GetBonusPointRequest(int accountId)
		{
			AccountId = accountId;
		}

		public int AccountId { get; }
	}
}