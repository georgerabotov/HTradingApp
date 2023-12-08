using System;
using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.Requests
{
	public class AddCreditRequest : IRequest<BonusPointsResponse>
    {
        public int AccountId { get; set; }
    }
}