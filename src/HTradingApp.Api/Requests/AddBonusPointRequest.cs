using System;
using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.Requests
{
	public class AddBonusPointRequest : IRequest<bool>
    {
        public int AccountId { get; set; }
        public int Amount { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }
}