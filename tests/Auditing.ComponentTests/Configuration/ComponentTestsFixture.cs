﻿using EasyNetQ;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Auditing.ComponentTests.Configuration;

public class ComponentTestsFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _application = CustomWebApplicationFactory<Program>.Instance;
    private readonly DockerInfrastructureFixture _infrastructure = new();

    public RepositoryTestDouble RepositoryTestDouble { get; private set; } = null!;
    public MessagesPublisher MessagesPublisher { get; private set; } = null!;

    public Task InitializeAsync()
    {
        RepositoryTestDouble = _application.Services.GetRequiredService<RepositoryTestDouble>();
        var bus = _application.Services.GetRequiredService<IBus>();
        MessagesPublisher = new MessagesPublisher(bus);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _application.DisposeAsync();
        await _infrastructure.DisposeAsync();
    }
}
