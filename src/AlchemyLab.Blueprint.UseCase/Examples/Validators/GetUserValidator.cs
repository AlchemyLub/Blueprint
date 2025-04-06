namespace AlchemyLab.Blueprint.UseCase.Examples.Validators
{
    /// <summary>
    /// Валидатор для запроса получения пользователя
    /// </summary>
    public class GetUserValidator : IValidator<GetUserRequest>
    {
        /// <inheritdoc />
        public Task<ValidationResult> ValidateAsync(GetUserRequest request, CancellationToken cancellationToken = default)
        {
            ValidationResult result = new();

            if (request.UserId == Guid.Empty)
            {
                result.AddError($"Идентификатор пользователя не может быть пустым (UserId = {request.UserId})");
            }

            return Task.FromResult(result);
        }
    }
}
