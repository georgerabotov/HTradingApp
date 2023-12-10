using System.Reflection;
using FluentValidation;
using HTradingApp.Api.ControllerModels;
using HTradingApp.Api.Core;
using HTradingApp.Api.Requests;
using HTradingApp.Api.Requests.Validators;
using HTradingApp.Domain;
using HTradingApp.Mock.Services;
using HTradingApp.Persistence.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<IAccounts, AccountService>();
builder.Services.AddScoped<IDeals, DealService>();
builder.Services.AddScoped<ICreditOperations, CreditService>();
builder.Services.AddSingleton<DataInitiliazer>();

builder.Services.AddScoped<IBonusService, BonusService>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddTransient<ErrorHandlingMiddleWare>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IValidator<AddBonusPointRequest>, AddBonusPointValidator>();
builder.Services.AddScoped<IValidator<AddCreditRequest>, AddCreditValidator>();
builder.Services.AddScoped<IValidator<GetBonusPointRequest>, GetBonusPointValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleWare>();

// Load fake data into cache
var initializationService = app.Services.GetRequiredService<DataInitiliazer>();
initializationService.GenerateFakeData(); 

app.MapControllers();

app.Run();