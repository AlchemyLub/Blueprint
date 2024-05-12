namespace AlchemyLub.Blueprint.Endpoints.Validators;

/// <summary>
/// Валидатор для <see cref="EntityRequest"/>
/// </summary>
public class EntityRequestValidator : AbstractValidator<EntityRequest>
{
    public EntityRequestValidator() => RuleFor(t => t.Title).NotEmpty();
}
