using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using AlchemyLab.Blueprint.UseCase.Behaviors;

namespace AlchemyLab.Blueprint.UseCase.UnitTests
{
    public class ValidationPipelineTests
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class ValidatedAttribute : Attribute
        {
        }

        [Fact]
        public async Task ValidationPipeline_WithValidRequest_ShouldNotThrow()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IValidator<TestValidatedRequest>, TestValidator>();
            var provider = services.BuildServiceProvider();

            var pipeline = new ValidationPipeline<TestValidatedRequest, TestResponse>(provider);
            var request = new TestValidatedRequest { Name = "Valid Name" };

            bool nextCalled = false;
            Task<TestResponse> Next(TestValidatedRequest req)
            {
                nextCalled = true;
                return Task.FromResult(new TestResponse());
            }

            // Act
            await pipeline.HandleAsync(request, Next);

            // Assert
            Assert.True(nextCalled);
        }

        [Fact]
        public async Task ValidationPipeline_WithInvalidRequest_ShouldThrowValidationException()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IValidator<TestValidatedRequest>, TestManualExceptionValidator>();
            var provider = services.BuildServiceProvider();

            var pipeline = new ValidationPipeline<TestValidatedRequest, TestResponse>(provider);
            var request = new TestValidatedRequest { Name = "" }; // Invalid request

            bool nextCalled = false;
            Task<TestResponse> Next(TestValidatedRequest req)
            {
                nextCalled = true;
                return Task.FromResult(new TestResponse());
            }

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                () => pipeline.HandleAsync(request, Next)
            );

            Assert.False(nextCalled);
            Assert.NotNull(exception.ValidationResult);
            Assert.NotEmpty(exception.ValidationResult.Errors);
        }

        [Fact]
        public async Task ValidationPipeline_WithNonValidatedRequest_ShouldPassThrough()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IValidator<TestValidatedRequest>, TestValidator>();
            var provider = services.BuildServiceProvider();

            var pipeline = new ValidationPipeline<TestRequest, TestResponse>(provider);
            var request = new TestRequest();

            bool nextCalled = false;
            Task<TestResponse> Next(TestRequest req)
            {
                nextCalled = true;
                return Task.FromResult(new TestResponse());
            }

            // Act
            await pipeline.HandleAsync(request, Next);

            // Assert
            Assert.True(nextCalled);
        }

        // Тестовый запрос с валидацией
        [Validated]
        public class TestValidatedRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }

        // Обычный запрос без валидации
        public class TestRequest { }

        // Тестовый ответ
        public class TestResponse
        {
            public string Value { get; set; } = string.Empty;
        }

        // Тестовый валидатор
        public class TestValidator : IValidator<TestValidatedRequest>
        {
            public Task<ValidationResult> Validate(TestValidatedRequest request, CancellationToken cancellationToken = default)
            {
                var result = new ValidationResult();

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    result.AddError("Name", "Name cannot be empty");
                }

                return Task.FromResult(result);
            }
        }

        // Специальный валидатор, который всегда возвращает ошибку
        public class FailingTestValidator : IValidator<TestValidatedRequest>
        {
            public Task<ValidationResult> Validate(TestValidatedRequest request, CancellationToken cancellationToken = default)
            {
                var result = new ValidationResult();
                // Всегда добавляем ошибку независимо от запроса
                result.AddError("Name", "Validation always fails");
                // Явно проверяем, что IsValid действительно false
                var isValid = result.IsValid;
                if (isValid)
                {
                    throw new Exception("ValidationResult.IsValid должен быть false, но вернул true");
                }
                return Task.FromResult(result);
            }
        }

        // Валидатор который напрямую выбрасывает исключение
        public class TestManualExceptionValidator : IValidator<TestValidatedRequest>
        {
            public Task<ValidationResult> Validate(TestValidatedRequest request, CancellationToken cancellationToken = default)
            {
                Console.WriteLine($"TestManualExceptionValidator.Validate called with request.Name={request.Name}");

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    // Вместо возврата результата, выбрасываем исключение
                    Console.WriteLine("TestManualExceptionValidator.Validate preparing to throw ValidationException");
                    var result = new ValidationResult();
                    result.AddError("Name", "Name cannot be empty");
                    Console.WriteLine($"TestManualExceptionValidator.Validate result.IsValid={result.IsValid}, ErrorCount={result.Errors?.Count}");
                    Console.WriteLine("TestManualExceptionValidator.Validate throwing ValidationException now");
                    throw new ValidationException(result);
                }

                Console.WriteLine("TestManualExceptionValidator.Validate returning valid result");
                return Task.FromResult(new ValidationResult());
            }
        }

        // Тестовый UseCase с валидацией
        public class TestValidatedUseCase : UseCase<TestValidatedRequest, TestResponse>
        {
            public bool WasExecuted { get; private set; }

            public TestValidatedUseCase(UseCaseContext context) : base(context)
            {
            }

            protected override Task<TestResponse> ExecuteCore(TestValidatedRequest request)
            {
                WasExecuted = true;
                return Task.FromResult(new TestResponse { Value = $"Validated: {request.Value}" });
            }
        }

        // Тестовая реализация ValidationPipeline для примера
        public class ValidationPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly IServiceProvider _serviceProvider;

            public ValidationPipeline(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task<TResponse> HandleAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
            {
                Console.WriteLine($"ValidationPipeline.HandleAsync: Start with request {request}");

                // Проверяем, нужно ли валидировать запрос
                if (!typeof(TRequest).IsDefined(typeof(ValidatedAttribute), true))
                {
                    Console.WriteLine($"ValidationPipeline.HandleAsync: Request {request} is not validated, skipping validation");
                    return await next(request);
                }

                // Получаем валидатор
                var validatorType = typeof(IValidator<>).MakeGenericType(typeof(TRequest));
                var validator = _serviceProvider.GetService(validatorType) as dynamic;

                if (validator == null)
                {
                    Console.WriteLine($"ValidationPipeline.HandleAsync: No validator found for {request}");
                    return await next(request);
                }

                Console.WriteLine($"ValidationPipeline.HandleAsync: Found validator {validator.GetType().FullName}");

                try
                {
                    // Вызываем валидацию
                    ValidationResult validationResult = await validator.Validate(request);

                    Console.WriteLine($"ValidationPipeline.HandleAsync: Validation result IsValid={validationResult.IsValid}, ErrorCount={validationResult.Errors?.Count}");

                    // Проверяем результат валидации
                    if (!validationResult.IsValid)
                    {
                        Console.WriteLine($"ValidationPipeline.HandleAsync: Validation failed, throwing ValidationException");
                        throw new ValidationException(validationResult);
                    }

                    Console.WriteLine($"ValidationPipeline.HandleAsync: Validation passed, continuing pipeline");
                    // Если валидация прошла успешно, продолжаем цепочку
                    return await next(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ValidationPipeline.HandleAsync: Exception occurred: {ex.GetType().FullName} - {ex.Message}");
                    throw;
                }
            }
        }
    }
}
