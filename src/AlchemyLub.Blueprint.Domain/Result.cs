namespace AlchemyLub.Blueprint.Domain;

public readonly record struct Result
{
    private Result(bool succeeded, string[] errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static Result Success() => new(true, []);

    public static Result Failure(string[] errors) => new(false, errors);
}
