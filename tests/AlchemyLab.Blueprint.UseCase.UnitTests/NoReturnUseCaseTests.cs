namespace AlchemyLab.Blueprint.UseCase.UnitTests
{
    public class NoReturnUseCaseTests
    {
        [Fact]
        public async Task Execute_NoReturnUseCase_ShouldExecuteCore()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestCommandRequest { Value = "Command" };

            // Используем NSubstitute для отслеживания вызовов
            var mockExecutor = Substitute.For<ITestCommandExecutor>();
            services.AddSingleton(mockExecutor);
            services.AddSingleton<UseCaseContext>();
            services.AddUseCase<TestCommandUseCase>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestCommandUseCase>();

            // Act
            await useCase.Execute(request);

            // Assert - проверяем, что был вызван Execute с правильным значением
            mockExecutor.Received(1).Execute(Arg.Is<TestCommandRequest>(r => r.Value == "Command"));
            Assert.True(useCase.Executed);
        }

        [Fact]
        public async Task Execute_NoReturnUseCase_WithPipeline_ShouldApplyPipeline()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestCommandRequest { Value = "Command" };

            var mockExecutor = Substitute.For<ITestCommandExecutor>();
            services.AddSingleton(mockExecutor);
            services.AddSingleton<UseCaseContext>();

            services.AddUseCase<TestCommandUseCase>();
            services.AddPipeline<TestCommandPipeline>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestCommandUseCase>();

            // Act
            await useCase.Execute(request);

            // Assert - проверяем, что был вызван Execute с модифицированным значением
            mockExecutor.Received(1).Execute(Arg.Is<TestCommandRequest>(r => r.Value == "Command - modified"));
        }

        [Fact]
        public async Task Interface_NoReturnUseCase_ShouldExecuteCore()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestCommandRequest { Value = "Command" };

            var mockExecutor = Substitute.For<ITestCommandExecutor>();
            services.AddSingleton(mockExecutor);
            services.AddSingleton<UseCaseContext>();

            services.AddUseCase<TestCommandUseCase>();

            using var serviceProvider = services.BuildUseCaseProvider();

            // Получаем UseCase через интерфейс IUseCase<>
            var useCase = serviceProvider.GetRequiredService<IUseCase<TestCommandRequest>>();

            // Act
            await useCase.Execute(request);

            // Assert
            mockExecutor.Received(1).Execute(Arg.Is<TestCommandRequest>(r => r.Value == "Command"));
        }

        #region Test Classes

        public interface ITestCommandExecutor
        {
            void Execute(TestCommandRequest command);
        }

        public class TestCommandRequest
        {
            public string Value { get; set; } = string.Empty;
        }

        public class TestCommandUseCase : UseCase<TestCommandRequest>
        {
            private readonly ITestCommandExecutor _executor;
            public bool Executed { get; private set; }

            public TestCommandUseCase(ITestCommandExecutor executor, UseCaseContext context) : base(context)
            {
                _executor = executor;
            }

            protected override Task<ValueTuple> ExecuteCore(TestCommandRequest request)
            {
                Executed = true;
                _executor.Execute(request);
                return Task.FromResult(new ValueTuple());
            }
        }

        [GlobalPipeline]
        public class TestCommandPipeline : IPipeline<TestCommandRequest, ValueTuple>
        {
            public async Task<ValueTuple> HandleAsync(
                TestCommandRequest request,
                Func<TestCommandRequest, Task<ValueTuple>> next)
            {
                // Модифицируем запрос
                request.Value += " - modified";

                // Передаем управление следующему элементу в цепочке
                return await next(request);
            }
        }

        #endregion
    }
}
