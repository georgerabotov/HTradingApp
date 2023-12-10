using System;
using FluentValidation.TestHelper;
using FluentAssertions;
using HTradingApp.Api.Requests;
using HTradingApp.Api.Requests.Validators;
using HTradingApp.Mock.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using HTradingApp.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using HTradingApp.Api.ControllerModels;

namespace HTradingApp.UnitTests
{
	public class ValidatorTests
	{
		private readonly AddBonusPointValidator _addBonusValidator;
		private readonly AddCreditValidator _addCreditValidator;
		private readonly GetBonusPointValidator _getBonusValidator;
        private readonly DataInitiliazer _dataInitializer;
        private readonly IMemoryCache _cache;

		public ValidatorTests()
		{
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetService<IMemoryCache>();

            _dataInitializer = new DataInitiliazer(_cache);
            _addBonusValidator = new AddBonusPointValidator(_cache);
            _addCreditValidator = new AddCreditValidator(_cache);
            _getBonusValidator = new GetBonusPointValidator(_cache);
        }

		[Fact]
		public void AddBonusPointValidator_Should_Be_True()
		{
            // Arrange
            _dataInitializer.GenerateFakeData();
			var model = new AddBonusPointRequest(1, DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1));

			// Act
			var result = _addBonusValidator.TestValidate(model);

			// Assert
			result.IsValid.Should().BeTrue();
		}

        [Fact]
        public void AddBonusPointValidator_Not_Found_AccountId()
        {
            // Arrange
            _dataInitializer.GenerateFakeData();
            var model = new AddBonusPointRequest(6, DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(1));

            // Act
            var result = _addBonusValidator.TestValidate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Account does not exist");
        }

        [Fact]
        public void AddBonusPointValidator_FromDate_Null_Date()
        {
            // Arrange
            _dataInitializer.GenerateFakeData();
            var model = new AddBonusPointRequest(1, DateTime.MinValue, DateTime.Now.AddMonths(1));

            // Act
            var result = _addBonusValidator.TestValidate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.FromDateTime).WithErrorMessage("'From Date Time' must not be empty.");
        }

        [Fact]
        public void AddBonusPointValidator_ToDate_Null_Date()
        {
            // Arrange
            _dataInitializer.GenerateFakeData();
            var model = new AddBonusPointRequest(1, DateTime.Now.AddMonths(1), DateTime.MinValue);

            // Act
            var result = _addBonusValidator.TestValidate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.ToDateTime).WithErrorMessage("'To Date Time' must not be empty.");
        }

        [Fact]
        public void AddCreditValidator_Not_Found_AccountId()
        {
            // Arrange
            _dataInitializer.GenerateFakeData();
            var model = new AddCreditRequest(6);

            // Act
            var result = _addCreditValidator.TestValidate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Account does not exist");
        }

        [Fact]
        public void GetBonusPointsValidator_Not_Found_AccountId()
        {
            // Arrange
            _dataInitializer.GenerateFakeData();
            var model = new GetBonusPointRequest(6);

            // Act
            var result = _getBonusValidator.TestValidate(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(x => x.AccountId).WithErrorMessage("Account does not exist");
        }
    }
}

