using System;
using HTradingApp.Api.Requests.Responses;
using MediatR;

namespace HTradingApp.Api.Requests
{
	public class AddCreditRequest : IRequest<bool>
    {
        public AddCreditRequest(int accountId)
        {
            AccountId = accountId;
        }
        public int AccountId { get; }
    }
}