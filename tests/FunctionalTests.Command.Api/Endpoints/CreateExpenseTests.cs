using System.Net;
using System.Net.Http.Json;
using Assessments.ExpenseManagement.Command.Api.Models;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Abstractions;
using Assessments.ExpenseManagement.FunctionalTests.Command.Api.Fixtures;
using Assessments.ExpenseManagement.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Assessments.ExpenseManagement.FunctionalTests.Command.Api.Endpoints;

public sealed class CreateExpenseTests(TestWebApplicationFactory factory) : TestBase(factory)
{
    [Fact]
    public async Task Should_ReturnCreated_When_Successful()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.EUR;
        var user = UserPresets.JaneDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateExpenseResponse>();
        result.ShouldBeOfType<CreateExpenseResponse>().Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Should_ReturnNotFound_With_ProblemDetails_When_CategoryNotFound()
    {
        // Arrange
        const string categoryName = "Whatever";
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            categoryName,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_With_ProblemDetails_When_CurrencyNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        const string currencyCode = "JPY";
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currencyCode,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Should_ReturnNotFound_With_ProblemDetails_When_UserNotFound()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var userId = Guid.NewGuid();
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            userId);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_With_ProblemDetails_When_DescriptionIsEmpty()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            string.Empty,
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status412PreconditionFailed);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_With_ProblemDetails_When_ExecutionDateIsDatedMoreThanThreeMonths()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now.AddMonths(-3).AddDays(-1)),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status412PreconditionFailed);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_With_ProblemDetails_When_ExecutionDateIsFutureDate()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status412PreconditionFailed);
    }

    [Fact]
    public async Task
        Should_ReturnPreconditionFailed_With_ProblemDetails_When_ExpenseCurrencyIsNotAlignedWithUserCurrency()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.EUR; // not aligned with user currency
        var user = UserPresets.JohnDoe; // user currency is USD
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status412PreconditionFailed);
    }

    [Fact]
    public async Task Should_ReturnPreconditionFailed_With_ProblemDetails_When_ExpenseIsDuplicated()
    {
        // Arrange
        var category = CategoryPresets.Hotel;
        var currency = CurrencyPresets.USD;
        var user = UserPresets.JohnDoe;
        var request = new CreateExpenseRequest(
            100M,
            category.Name,
            currency.Code,
            nameof(CreateExpenseRequest.Description),
            DateOnly.FromDateTime(DateTime.Now),
            user.Id);

        _ = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/expenses", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var problemDetails = content.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status412PreconditionFailed);
    }
}
