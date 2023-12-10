using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HTradingApp.Api.Requests
{
    public class AddCreditRequest : IRequest<IActionResult>
    {
        public AddCreditRequest(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }
    }
}