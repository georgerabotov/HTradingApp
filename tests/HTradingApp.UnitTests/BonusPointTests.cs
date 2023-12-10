using Bogus;
using FluentAssertions;
using HTradingApp.Domain.Models;
using HTradingApp.Mock.Services;
using HTradingApp.Persistence.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HTradingApp.UnitTests;

public class BonusPointTests
{
    private readonly IMemoryCache _cache;

    public BonusPointTests(IMemoryCache cache)
    {
        _cache = cache;
    }

    [Fact]
    public async void GetAccountBonusPoints_Should_Return_Account_Bonus_Points()
    {
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        int accountId = 1;
        BonusService service = new(_cache);

        var result = await service.GetAccountBonusPoints(accountId);
        result.Should().NotBe(0);
    }

    [Fact]
    public async void GetAccountBonusPoints_Should_Return_0_If_No_Bonus_Points()
    {
        // Red - return correct amount
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        int accountId = 1;
        BonusService service = new(_cache);

        var result = await service.GetAccountBonusPoints(accountId);
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(1, 10, true)]
    [InlineData(1, 1000, false)]
    public async void AddAccountBonusPoints_Should_Return_Correct_On_Input(int accountId, int bonusPoints, bool expectedResult)
    {
        // still on red - If successfully added, return true, otherwise, false
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        BonusService service = new(_cache);

        var result = await service.AddAccountBonusPoints(accountId, bonusPoints);
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1, 5, 1000, 5)]
    [InlineData(2, 10, 10000, 20)]
    [InlineData(3, 5, 100000, 35)]
    [InlineData(4, 5, 1000000, 65)]
    [InlineData(4, 10, 10000000, 115)]
    public async void CalculateBonusPoints_Should_Return_Correct_Amount(int accountId, int numberOfDeals, decimal dealAmount, int expectedResult)
    {
        // Currently I haven't written the algorithm for the application but the idea is
        // Deal count > 10 += 5pts
        // amount > 1000 and < 10000 += 5 pts
        // Amount > 10000 += 10pts
        // Amount > 100000 += 20pts
        // Amount > 1000000 += 30pts
        // Amount > 10000000 += 45pts

        var deals = new List<Deal>();
        deals.AddRange(
            new Faker<Deal>()
            .RuleFor(x => x.AccountId, accountId)
            .RuleFor(x => x.Amount, dealAmount)
            .RuleFor(x => x.Id, Guid.NewGuid())
            .RuleFor(x => x.DealDateTime, DateTime.Now)
            .Generate(numberOfDeals));

        BonusService service = new(_cache);

        var result = await service.CalculateBonusPoints(accountId, deals);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(6, false)]
    public async void FlushBonusPoints_Should_Return_Correct_Result(int accountId, bool expectedResult)
    {
        // Still on red - bonus points will be flushed, if successful - true, else - false
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        BonusService service = new(_cache);

        var result = service.FlushBonusPoints(accountId);
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(5, 50)]
    [InlineData(20, 200)]
    [InlineData(35, 350)]
    [InlineData(65, 650)]
    [InlineData(115, 1150)]

    public async void CalculateAccountCredit_Should_Return_Correct_Credit(int bonusPoints, int expectedCreditAmount)
    {
        // still on red - for each bonus point, $10 will be credited
        BonusService service = new(_cache);

        var result = await service.CalculateAccountCredit(bonusPoints);
        result.Should().Be(expectedCreditAmount);
    }

    [Theory]
    [InlineData(1, 1, false)]
    [InlineData(2, 5, true)]
    public async void IsEligibleForBonusPoints_Should_Return_Correct_Result(int accountId, int numberOfDeals, bool expectedResult)
    {
        // still on red - if deals are less than 5, not eligible for bonus points
        var deals = new List<Deal>();
        deals.AddRange(
            new Faker<Deal>()
            .RuleFor(x => x.AccountId, accountId)
            .RuleFor(x => x.Amount, y => y.Random.Decimal())
            .RuleFor(x => x.Id, Guid.NewGuid())
            .RuleFor(x => x.DealDateTime, DateTime.Now)
            .Generate(numberOfDeals));

        BonusService service = new(_cache);

        var result = await service.IsEligibleForBonusPoints(accountId, deals);

        result.Should().Be(expectedResult);
    }
}
