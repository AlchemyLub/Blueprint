namespace AlchemyLub.Blueprint.Endpoints.Filters;

/// <summary>
/// Action filter for automatic validation of action method arguments.
/// </summary>
public class AutoValidationEndpointFilter : IEndpointFilter, IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ValidationResult validationResult = await ValidateArguments(
            context.HttpContext.RequestServices,
            context.ActionArguments.Select(t => t.Value).ToList(),
            context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            context.Result = new ObjectResult(CreateValidationProblemDetails(validationResult));

            return;
        }

        await next();
    }

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        ValidationResult validationResult = await ValidateArguments(
            context.HttpContext.RequestServices,
            context.Arguments,
            context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(CreateValidationProblemDetails(validationResult));
        }

        return await next(context);
    }

    private static async Task<ValidationResult> ValidateArguments(
        IServiceProvider serviceProvider,
        IList<object?> arguments,
        CancellationToken cancellationToken = default)
    {
        ValidationResult result = new();

        foreach (object? argument in arguments)
        {
            if (argument is null)
            {
                continue;
            }

            Type argumentType = argument.GetType();

            if (!argumentType.IsCustomType())
            {
                continue;
            }

            Type validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

            object? validatorObject = serviceProvider.GetService(validatorType);

            if (validatorObject is IValidator validator)
            {
                IValidationContext validationContext = new ValidationContext<object>(argument);

                ValidationResult validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

                result.Errors.AddRange(validationResult.Errors);
            }
        }

        return result;
    }

    private static ProblemDetails CreateValidationProblemDetails(ValidationResult validationResult) =>
        new()
        {
            Title = "Validation Error",
            Detail = "One or more validation errors occurred.",
            Status = 400,
            Extensions =
            {
                { "errors", validationResult.ToDictionary() }
            }
        };
}
