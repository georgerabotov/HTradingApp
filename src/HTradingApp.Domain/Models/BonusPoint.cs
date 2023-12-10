using System;
namespace HTradingApp.Domain.Models
{
	public class BonusPoint
	{
		public Guid Id { get; set; }
        public int AccountId { get; set; }
		public int Amount { get; set; }
		public DateTime? BonusAdded { get; set; }
		public bool ConvertedToCredit { get; set; }
    }
}