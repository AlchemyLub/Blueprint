using System;
using System.Threading;
using System.Threading.Tasks;
using AlchemyLab.Blueprint.UseCase.Pipelines;

namespace AlchemyLab.Blueprint.UseCase.Examples.Validators
{
    /// <summary>
    /// Валидатор для запроса обновления пользователя
    /// </summary>
    public class UpdateUserValidator : IValidator<UpdateUserRequest>
    {
        /// <inheritdoc />
        public Task<ValidationResult> ValidateAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();

            if (request.UserId == Guid.Empty)
            {
                result.AddError("Идентификатор пользователя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                result.AddError("Имя пользователя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("Email пользователя не может быть пустым");
            }
            else if (!request.Email.Contains('@'))
            {
                result.AddError("Email пользователя должен содержать символ @");
            }

            return Task.FromResult(result);
        }
    }
}
