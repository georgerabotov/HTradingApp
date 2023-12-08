using System;
namespace HTradingApp.Domain
{
	public interface ICreditOperations
	{
        bool CreateCreditOperation(int accountId, decimal amount);
    }
}