using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace AlchemyLab.Blueprint.UseCase.UnitTests
{
    public class UseCaseContextTests
    {
        private readonly Type _useCaseContextType;
        private readonly MethodInfo _initializeMethod;
        private readonly MethodInfo _applyPipelinesMethod;
        private readonly FieldInfo _serviceProviderField;

        public UseCaseContextTests()
        {
            // Получаем тип UseCaseContext через рефлексию, так как он internal
            _useCaseContextType = typeof(UseCase<,>).Assembly.GetType("AlchemyLab.Blueprint.UseCase.UseCaseContext")!;
            _initializeMethod = _useCaseContextType.GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public)!;
            _applyPipelinesMethod = _useCaseContextType.GetMethod("ApplyPipelines", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public)!;
            _serviceProviderField = _useCaseContextType.GetField("serviceProvider", BindingFlags.NonPublic | BindingFlags.Static)!;
        }

        [Fact]
        public void Initialize_WithValidServiceProvider_ShouldNotThrow()
        {
            // Arrange
            var serviceProvider = Substitute.For<IServiceProvider>();

            // Сбрасываем состояние статического поля перед тестом
            _serviceProviderField.SetValue(null, null);

            // Act & Assert
            var exception = Record.Exception(() => _initializeMethod.Invoke(null, new object[] { serviceProvider }));
            Assert.Null(exception);

            // Проверяем, что сервис-провайдер был установлен
            var actualServiceProvider = _serviceProviderField.GetValue(null);
            Assert.Same(serviceProvider, actualServiceProvider);
        }

        [Fact]
        public async Task ApplyPipelines_WithNoApplicablePipelines_ShouldExecuteCore()
        {
            // Arrange
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            // Сбрасываем состояние статического поля перед тестом
            _serviceProviderField.SetValue(null, null);

            // Инициализируем контекст с мок-сервис-провайдером
            _initializeMethod.Invoke(null, new object[] { serviceProvider });

            // Проверяем, что сервис-провайдер был установлен
            var actualServiceProvider = _serviceProviderField.GetValue(null);
            Assert.NotNull(actualServiceProvider);

            var request = new TestRequest();
            var expectedResponse = new TestResponse();
            bool executeCoreWasCalled = false;

            Task<TestResponse> ExecuteCore(TestRequest req)
            {
                executeCoreWasCalled = true;
                return Task.FromResult(expectedResponse);
            }

            // Act
            var method = _applyPipelinesMethod.MakeGenericMethod(typeof(TestRequest), typeof(TestResponse));
            var result = await (Task<TestResponse>)method.Invoke(null, new object[] {
                request,
                new Func<TestRequest, Task<TestResponse>>(ExecuteCore),
                typeof(TestUseCase)
            })!;

            // Assert
            Assert.True(executeCoreWasCalled);
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task ApplyPipelines_WithMockedServiceProvider_ShouldCallPipelines()
        {
            // Arrange
            bool pipelineWasCalled = false;

            // Создаем коллекцию сервисов и регистрируем мок-пайплайн
            var services = new ServiceCollection();

            // Создаем мок-пайплайн с атрибутом GlobalPipeline
            var pipeline = Substitute.For<IPipeline<TestRequest, TestResponse>>();
            pipeline.HandleAsync(Arg.Any<TestRequest>(), Arg.Any<Func<TestRequest, Task<TestResponse>>>())
                .Returns(callInfo =>
                {
                    pipelineWasCalled = true;
                    var next = callInfo.ArgAt<Func<TestRequest, Task<TestResponse>>>(1);
                    return next(callInfo.ArgAt<TestRequest>(0));
                });

            // Добавляем атрибут GlobalPipeline к типу пайплайна через рефлексию
            var pipelineType = pipeline.GetType();
            var globalPipelineAttrCtor = typeof(GlobalPipelineAttribute).GetConstructor(Type.EmptyTypes)!;
            var globalPipelineAttr = new CustomAttributeBuilder(globalPipelineAttrCtor, new object[] { });

            // К сожалению, нельзя добавить атрибут к прокси-типу NSubstitute, поэтому используем другой подход
            // Регистрируем пайплайн напрямую в контейнере
            services.AddSingleton<IPipeline<TestRequest, TestResponse>>(pipeline);

            var serviceProvider = services.BuildServiceProvider();

            // Сбрасываем состояние статического поля перед тестом
            _serviceProviderField.SetValue(null, null);

            // Инициализируем контекст с созданным сервис-провайдером
            _initializeMethod.Invoke(null, new object[] { serviceProvider });

            var request = new TestRequest();
            var expectedResponse = new TestResponse();

            Task<TestResponse> ExecuteCore(TestRequest req)
            {
                return Task.FromResult(expectedResponse);
            }

            // Act
            var method = _applyPipelinesMethod.MakeGenericMethod(typeof(TestRequest), typeof(TestResponse));
            var result = await (Task<TestResponse>)method.Invoke(null, new object[] {
                request,
                new Func<TestRequest, Task<TestResponse>>(ExecuteCore),
                typeof(TestUseCase)
            })!;

            // Assert
            Assert.True(pipelineWasCalled);
            Assert.Equal(expectedResponse, result);
        }

        /// <summary>
        /// Helper class to temporarily mark a pipeline with GlobalPipeline attribute
        /// </summary>
        private class GlobalPipelineScope : IDisposable
        {
            private readonly object pipeline;

            public GlobalPipelineScope(object pipeline)
            {
                this.pipeline = pipeline;
                // В реальном коде здесь было бы добавление атрибута к типу,
                // но для тестирования мы просто переопределяем метод ShouldApplyToPipeline
            }

            public void Dispose()
            {
                // В реальном коде здесь было бы удаление атрибута
            }
        }

        public class TestRequest { }

        public class TestResponse { }

        public class TestUseCase : UseCase<TestRequest, TestResponse>
        {
            // Обратите внимание на constructor-chaining here
            public TestUseCase() : this(null)
            {
            }

            public TestUseCase(UseCaseContext context) : base(context)
            {
            }

            protected override Task<TestResponse> ExecuteCore(TestRequest request)
            {
                return Task.FromResult(new TestResponse());
            }
        }
    }
}
