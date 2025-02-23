using AutoFixture.Xunit2;
using FluentValidation;

using Location.Application.Behaviours;

using Microsoft.Extensions.Logging;

namespace Application.Test.Behaviours;

public class ValidationBehaviorTest
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

    [Theory]
    [AutoData]
    public async Task Handle_WithNoValidators_ShouldReturnNextResult(TestQuery request)
    {
        // Arrange
        var logger = _fixture.Freeze<ILogger<ValidationBehavior<TestQuery, Result<TestResponse>>>>();
        var validators = new List<IValidator<TestQuery>>();
        var behavior = new ValidationBehavior<TestQuery, Result<TestResponse>>(validators, logger);
        var response = new TestResponse { Id = 1 };
        var expectedResult = Result<TestResponse>.Success(response);

        // Act
        var result = await behavior.Handle(
            request,
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(response);
    }

    [Theory]
    [AutoData]
    public async Task Handle_WithValidators_NoErrors_ShouldReturnNextResult(TestQuery request)
    {
        // Arrange
        var logger = _fixture.Freeze<ILogger<ValidationBehavior<TestQuery, Result<TestResponse>>>>();
        var validator = new TestValidator();
        var validators = new List<IValidator<TestQuery>> { validator };
        var behavior = new ValidationBehavior<TestQuery, Result<TestResponse>>(validators, logger);
        var response = new TestResponse { Id = 1 };
        var expectedResult = Result<TestResponse>.Success(response);

        request.Name = "Valid";

        // Act
        var result = await behavior.Handle(
            request,
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(response);
    }

    [Theory]
    [AutoData]
    public async Task Handle_WithMultipleValidators_NoErrors_ShouldReturnNextResult(TestQuery request)
    {
        // Arrange
        var logger = _fixture.Freeze<ILogger<ValidationBehavior<TestQuery, Result<TestResponse>>>>();
        var validator1 = new TestValidator();
        var validator2 = new TestValidator();
        var validators = new List<IValidator<TestQuery>> { validator1, validator2 };
        var behavior = new ValidationBehavior<TestQuery, Result<TestResponse>>(validators, logger);
        var response = new TestResponse { Id = 1 };
        var expectedResult = Result<TestResponse>.Success(response);

        request.Name = "Valid";

        // Act
        var result = await behavior.Handle(
            request,
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(response);
    }

    [Theory]
    [AutoData]
    public async Task Handle_WithValidationErrors_ShouldReturnInvalidResult(TestQuery request)
    {
        // Arrange
        var logger = _fixture.Freeze<ILogger<ValidationBehavior<TestQuery, Result<TestResponse>>>>();
        var validator = new TestValidator();
        var validators = new List<IValidator<TestQuery>> { validator };
        var behavior = new ValidationBehavior<TestQuery, Result<TestResponse>>(validators, logger);
        var response = new TestResponse { Id = 1 };
        var expectedResult = Result<TestResponse>.Success(response);

        request.Name = string.Empty;

        // Act
        var result = await behavior.Handle(
            request,
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Count().ShouldBe(1);
        result.ValidationErrors.First().ErrorMessage.ShouldBe("Name is required");
    }

    public class TestResponse
    {
        public int Id { get; set; }
    }

    public class TestValidator : AbstractValidator<TestQuery>
    {
        public TestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}