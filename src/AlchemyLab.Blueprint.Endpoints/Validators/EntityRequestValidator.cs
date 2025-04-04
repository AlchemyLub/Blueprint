namespace AlchemyLab.Blueprint.Endpoints.Validators;

/// <summary>
/// Валидатор для <see cref="EntityRequest"/>
/// </summary>
public sealed class EntityRequestValidator : AbstractValidator<EntityRequest>
{
    public EntityRequestValidator()
    {
        RuleFor(t => t.Title).NotEmpty();
        RuleFor(t => t.Count).GreaterThan(10);
    }
}
