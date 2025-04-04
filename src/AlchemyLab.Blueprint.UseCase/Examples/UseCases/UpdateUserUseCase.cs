using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlchemyLab.Blueprint.UseCase.Examples.UseCases
{
    /// <summary>
    /// Запрос для обновления пользователя
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Новое имя пользователя
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Новый email пользователя
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Пример UseCase'а для обновления пользователя (без возвращаемого значения)
    /// </summary>
    public class UpdateUser : UseCase<UpdateUserRequest>
    {
        /// <summary>
        /// Конструктор юзкейса
        /// </summary>
        /// <param name="context">Контекст юзкейса</param>
        public UpdateUser(UseCaseContext context) : base(context)
        {
        }

        /// <inheritdoc />
        protected override async Task<ValueTuple> ExecuteCore(UpdateUserRequest request)
        {
            // В реальном проекте здесь будет логика обновления пользователя в базе данных
            await Task.Delay(100); // Имитация задержки

            // Пример логирования или другой бизнес-логики
            Console.WriteLine($"Обновление пользователя {request.UserId}:");
            Console.WriteLine($"- Новое имя: {request.Name}");
            Console.WriteLine($"- Новый email: {request.Email}");

            return default;
        }
    }
}
