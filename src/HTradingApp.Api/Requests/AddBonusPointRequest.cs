using System;
using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.Requests
{
	public class AddBonusPointRequest : IRequest<bool>
    {
        public AddBonusPointRequest(int accountId, DateTime fromDate, DateTime toDate)
        {
            AccountId = accountId;
            FromDateTime = fromDate;
            ToDateTime = toDate;
        }
        public int AccountId { get; }
        public DateTime FromDateTime { get; }
        public DateTime ToDateTime { get; }
    }
}