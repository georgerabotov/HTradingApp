using System;
namespace HTradingApp.Domain.Models
{
	public class Deal
	{
		public Guid Id { get; set; }
		public int AccountId { get; set; }
		public decimal Amount { get; set; }
		public DateTime DealDateTime { get; set; }
    }
}

