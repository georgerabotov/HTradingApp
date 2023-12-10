using Bogus;
using FluentAssertions;
using HTradingApp.Domain.Models;
using HTradingApp.Mock.Services;
using HTradingApp.Persistence.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HTradingApp.UnitTests;

public class BonusPointTests
{
    private readonly IMemoryCache _cache;

    public BonusPointTests()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        _cache = serviceProvider.GetService<IMemoryCache>();
    }

    [Fact]
    public void GetAccountBonusPoints_Should_Return_Account_Bonus_Points()
    {
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        int accountId = 1;
        BonusService service = new(_cache);

        var result = service.GetAccountBonusPoints(accountId);
        result.Should().NotBe(0);
    }

    [Fact]
    public void GetAccountBonusPoints_Should_Return_0_If_No_Bonus_Points()
    {
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        // Id does not exist
        int accountId = 15;
        BonusService service = new(_cache);

        var result = service.GetAccountBonusPoints(accountId);
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(15, 10, false)]
    [InlineData(1, 1000, true)]
    public void AddAccountBonusPoints_Should_Return_Correct_On_Input(int accountId, int bonusPoints, bool expectedResult)
    {
        // We have no Id of 15 , it should fail.
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        BonusService service = new(_cache);

        var result = service.AddAccountBonusPoints(accountId, bonusPoints);
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1, 5, 1000, 5)]
    [InlineData(2, 10, 1250, 25)]
    [InlineData(3, 5, 100000, 35)]
    [InlineData(4, 5, 1000000, 65)]
    [InlineData(4, 10, 10000000, 120)]
    public void CalculateBonusPoints_Should_Return_Correct_Amount(int accountId, int numberOfDeals, decimal dealAmount, int expectedResult)
    {

        var deals = new List<Deal>();
        deals.AddRange(
            new Faker<Deal>()
            .RuleFor(x => x.AccountId, accountId)
            .RuleFor(x => x.Amount, dealAmount)
            .RuleFor(x => x.Id, Guid.NewGuid())
            .RuleFor(x => x.DealDateTime, DateTime.Now)
            .Generate(numberOfDeals));

        BonusService service = new(_cache);

        var result = service.CalculateBonusPoints(accountId, deals);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(6, true)]
    public void FlushBonusPoints_Should_Return_Correct_Result(int accountId, bool expectedResult)
    {
        DataInitiliazer dataInitializer = new(_cache);
        dataInitializer.GenerateFakeData();

        BonusService service = new(_cache);

        var result = service.FlushBonusPoints(accountId);
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(5, 62.5)]
    [InlineData(20, 250)]
    [InlineData(35, 437.5)]
    [InlineData(65, 812.5)]
    [InlineData(115, 1437.5)]

    public void CalculateAccountCredit_Should_Return_Correct_Credit(int bonusPoints, decimal expectedCreditAmount)
    {
        BonusService service = new(_cache);

        var result =  service.CalculateAccountCredit(bonusPoints);
        result.Should().Be(expectedCreditAmount);
    }

    [Theory]
    [InlineData(1, 1, false)]
    [InlineData(2, 5, true)]
    public void IsEligibleForBonusPoints_Should_Return_Correct_Result(int accountId, int numberOfDeals, bool expectedResult)
    {
        var deals = new List<Deal>();
        deals.AddRange(
            new Faker<Deal>()
            .RuleFor(x => x.AccountId, accountId)
            .RuleFor(x => x.Amount, y => y.Random.Decimal())
            .RuleFor(x => x.Id, Guid.NewGuid())
            .RuleFor(x => x.DealDateTime, DateTime.Now)
            .Generate(numberOfDeals));

        BonusService service = new(_cache);

        var result = service.IsEligibleForBonusPoints(accountId, deals);

        result.Should().Be(expectedResult);
    }
}
