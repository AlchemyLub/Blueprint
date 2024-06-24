namespace AlchemyLub.Blueprint.Endpoints.Filters;

public class AutoValidationEndpointFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        IServiceProvider serviceProvider = context.HttpContext.RequestServices;

        foreach (object? argument in context.ActionArguments.Select(t => t.Value))
        {
            if (argument is null)
            {
                continue;
            }

            if (argument.GetType().IsCustomType() &&
                serviceProvider.GetValidator(argument.GetType()) is IValidator validator)
            {
                IValidationContext validationContext = new ValidationContext<object>(argument);

                ValidationResult? validationResult =
                    await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                if (!validationResult.IsValid)
                {
                    context.Result = new ObjectResult(new ProblemDetails
                    {
                        Title = "Validation Error",
                        Detail = "One or more validation errors occurred.",
                        Status = 400,
                        Extensions =
                        {
                            { "errors", validationResult.ToValidationProblemErrors() }
                        }
                    });

                    return;
                }
            }
        }

        await next();
    }
}

static file class Extensions
{
    public static bool IsCustomType(this Type? type)
    {
        Type[] builtInTypes =
        [
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Enum)
        ];

        return type != null && type.IsClass && !type.IsEnum && !type.IsValueType && !type.IsPrimitive && !builtInTypes.Contains(type);
    }

    public static object? GetValidator(this IServiceProvider serviceProvider, Type type) =>
        serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(type));

    public static Dictionary<string, string[]> ToValidationProblemErrors(this ValidationResult validationResult) =>
        validationResult.Errors
            .GroupBy(validationFailure => validationFailure.PropertyName)
            .ToDictionary(
                validationFailureGrouping => validationFailureGrouping.Key,
                validationFailureGrouping => validationFailureGrouping
                    .Select(validationFailure => validationFailure.ErrorMessage)
                    .ToArray());
}
