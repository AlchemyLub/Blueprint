using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AlchemyLab.Blueprint.UseCase.UnitTests
{
    public class UseCaseTests
    {
        [Fact]
        public async Task Execute_WithoutPipelines_ShouldExecuteCore()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestRequest { Value = "Test" };

            services.AddSingleton<UseCaseContext>();
            services.AddUseCase<TestUseCase>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestUseCase>();

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.Equal("Processed: Test", result.Value);
            Assert.True(useCase.Executed);
            Assert.Equal("Test", useCase.LastExecutedValue);
        }

        [Fact]
        public async Task Execute_WithGlobalPipeline_ShouldApplyPipeline()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestRequest { Value = "Test" };

            services.AddSingleton<UseCaseContext>();
            services.AddUseCase<TestUseCase>();
            services.AddPipeline<TestGlobalPipeline>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestUseCase>();

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.Equal("Processed: Test - global pipeline", result.Value);
        }

        [Fact]
        public async Task Execute_WithSpecificPipeline_ShouldApplyPipelineOnlyToSpecificUseCase()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestRequest { Value = "Test" };

            services.AddSingleton<UseCaseContext>();
            services.AddUseCase<TestUseCase>();
            services.AddUseCase<AnotherTestUseCase>();
            services.AddPipelineFor<TestSpecificPipeline, TestUseCase>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestUseCase>();
            var anotherUseCase = serviceProvider.GetRequiredService<AnotherTestUseCase>();

            // Act
            var result = await useCase.Execute(request);
            var anotherResult = await anotherUseCase.Execute(request);

            // Assert
            Assert.Equal("Processed: Test - specific pipeline", result.Value);
            Assert.Equal("Another processed: Test", anotherResult.Value);
        }

        [Fact]
        public async Task Execute_WithMultiplePipelines_ShouldApplyInCorrectOrder()
        {
            // Arrange
            var services = new ServiceCollection();
            var request = new TestRequest { Value = "Test" };

            services.AddSingleton<UseCaseContext>();
            services.AddUseCase<TestUseCase>();
            services.AddPipeline<TestGlobalPipeline>();
            services.AddPipelineFor<TestSpecificPipeline, TestUseCase>();

            using var serviceProvider = services.BuildUseCaseProvider();
            var useCase = serviceProvider.GetRequiredService<TestUseCase>();

            // Act
            var result = await useCase.Execute(request);

            // Assert
            Assert.Equal("Processed: Test - global pipeline - specific pipeline", result.Value);
        }

        #region Test Classes

        public class TestRequest
        {
            public string Value { get; set; } = string.Empty;
        }

        public class TestResponse
        {
            public string Value { get; set; } = string.Empty;
        }

        public class TestUseCase : UseCase<TestRequest, TestResponse>
        {
            private bool executed;
            private string lastExecutedValue;

            public bool Executed => executed;
            public string LastExecutedValue => lastExecutedValue;

            public TestUseCase(UseCaseContext context) : base(context)
            {
            }

            protected override Task<TestResponse> ExecuteCore(TestRequest request)
            {
                executed = true;
                lastExecutedValue = request.Value;
                return Task.FromResult(new TestResponse { Value = $"Processed: {request.Value}" });
            }
        }

        public class AnotherTestUseCase : UseCase<TestRequest, TestResponse>
        {
            private bool executed;
            private string lastExecutedValue;

            public bool Executed => executed;
            public string LastExecutedValue => lastExecutedValue;

            public AnotherTestUseCase(UseCaseContext context) : base(context)
            {
            }

            protected override Task<TestResponse> ExecuteCore(TestRequest request)
            {
                executed = true;
                lastExecutedValue = request.Value;
                return Task.FromResult(new TestResponse { Value = $"Another processed: {request.Value}" });
            }
        }

        [GlobalPipeline]
        public class TestGlobalPipeline : IPipeline<TestRequest, TestResponse>
        {
            public async Task<TestResponse> HandleAsync(
                TestRequest request,
                Func<TestRequest, Task<TestResponse>> next)
            {
                // Модифицируем запрос
                request.Value += " - global pipeline";

                // Передаем управление следующему элементу в цепочке
                return await next(request);
            }
        }

        [UseCaseFilter(typeof(TestUseCase))]
        public class TestSpecificPipeline : IPipeline<TestRequest, TestResponse>
        {
            public async Task<TestResponse> HandleAsync(
                TestRequest request,
                Func<TestRequest, Task<TestResponse>> next)
            {
                // Модифицируем запрос
                request.Value += " - specific pipeline";

                // Передаем управление следующему элементу в цепочке
                return await next(request);
            }
        }

        #endregion
    }
}
