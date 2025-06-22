using Assessments.ExpenseManagement.Application.Extensions;
using Assessments.ExpenseManagement.Infrastructure;
using Assessments.ExpenseManagement.Infrastructure.Extensions;
using Assessments.ExpenseManagement.Query.Api.Endpoints;
using Assessments.ExpenseManagement.Query.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Docker Compose mode.
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("expense-management-db")));

// .NET Aspire mode.
// builder.AddSqlServerDbContext<ExpenseManagementContext>("expense-management-db");

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

// Add infrastructure services.
builder.Services.AddInfrastructure();

// Add application services.
builder.Services.AddApplication();

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
app.UseExceptionHandler();

app.MapCategoryQueryEndpoints();
app.MapCurrencyQueryEndpoints();
app.MapEmployeeQueryEndpoints();
app.MapEmployeeExpenseQueryEndpoints();
app.MapExpenseQueryEndpoints();

app.Run();
