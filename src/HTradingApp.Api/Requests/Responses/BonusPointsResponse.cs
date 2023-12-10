namespace HTradingApp.Api.Requests.Responses
{
	public class BonusPointsResponse
	{
		public BonusPointsResponse(int id, int total)
		{
			Id = id;
			TotalBonusPointsAmount = total;
		}

		public int Id { get; }
		public int TotalBonusPointsAmount { get; }
	}
}