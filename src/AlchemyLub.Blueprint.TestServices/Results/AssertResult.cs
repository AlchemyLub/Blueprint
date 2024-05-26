namespace AlchemyLub.Blueprint.TestServices.Results;

// TODO: Причесать модель
/// <summary>
/// Представляет собой агрегированный результат теста
/// </summary>
public readonly struct AssertResult()
{
    public bool IsSuccessful => Errors.Count == 0;

    private Collection<string> Errors { get; } = new();

    public AssertResult AddError(string errorMessage)
    {
        Errors.Add(errorMessage);

        return this;
    }

    public Collection<string> GetErrors() => Errors;

    public AssertResult Combine(AssertResult assertResult)
    {
        IReadOnlyCollection<string> errors = assertResult.GetErrors();

        if (errors.Count > 0)
        {
            foreach (string error in errors)
            {
                Errors.Add(error);
            }
        }

        return this;
    }
}
