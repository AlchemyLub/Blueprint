namespace AlchemyLub.Blueprint.ArchTests.Results;

/// <summary>
/// Представляет собой агрегированный результат теста
/// </summary>
internal readonly struct AssertResult()
{
    public bool IsSuccessful => Errors.Count == 0;

    private List<string> Errors { get; } = new();

    public AssertResult AddError(string errorMessage)
    {
        Errors.Add(errorMessage);

        return this;
    }

    public List<string> GetErrors() => Errors;

    public AssertResult Combine(AssertResult assertResult)
    {
        var errors = assertResult.GetErrors();

        if (errors.Count > 0)
        {
            Errors.AddRange(errors);
        }

        return this;
    }
}
