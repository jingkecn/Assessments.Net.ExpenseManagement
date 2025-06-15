using System.Text.Json.Serialization;
using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Infrastructure;
using Assessments.ExpenseManagement.Infrastructure.Extensions;
using Assessments.ExpenseManagement.Query.Api.Endpoints;
using Assessments.ExpenseManagement.Query.Api.Exceptions;
using Assessments.ExpenseManagement.Starter.ServiceDefaults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Docker Compose mode.
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("expense-management-db")));

// .NET Aspire mode.
// builder.AddSqlServerDbContext<ExpenseManagementContext>("expense-management-db");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Add infrastructure services.
builder.Services.AddInfrastructure();

// Add application services.
builder.Services.AddApplication();

// Add exception handler and problem details.
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Services.InitializeInfrastructure();
}

app.UseHttpsRedirection();

app.MapExpenseQueryEndpoints();

app.Run();
