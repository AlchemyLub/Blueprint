global using AlchemyLab.Blueprint.Application.Repositories;
global using AlchemyLab.Blueprint.Clients.Abstractions;
global using AlchemyLab.Blueprint.Clients.Extensions;
global using AlchemyLab.Blueprint.Domain;
global using AlchemyLab.Blueprint.Endpoints.Controllers;
global using AlchemyLab.Blueprint.IntegrationTests.Extensions;
global using AlchemyLab.Blueprint.IntegrationTests.Stubs;
global using FluentAssertions;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Refit;
global using ClientEntityResponse = AlchemyLab.Blueprint.Clients.Responses.EntityResponse;
global using ClientEntityRequest = AlchemyLab.Blueprint.Clients.Requests.EntityRequest;
