using System;
using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.ControllerModels
{
	public class GetBonusPointRequest : IRequest<BonusPointsResponse>
	{
		public int AccountId { get; set; }
	}
}

