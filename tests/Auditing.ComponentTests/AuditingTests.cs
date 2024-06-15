﻿using Auditing.ComponentTests.Configuration;
using Auditing.Host.Contracts;
using Auditing.Host.Repositories;
using FluentAssertions;
using Xunit;

namespace Auditing.ComponentTests;

[Collection(nameof(ComponentTestsCollection))]
public class AuditingTests
{
    private readonly MessagesPublisher _messagesPublisher;
    private readonly RepositoryTestDouble _repositoryTestDouble;

    public AuditingTests(ComponentTestsFixture componentTestsFixture)
    {
        _messagesPublisher = componentTestsFixture.MessagesPublisher;
        _repositoryTestDouble = componentTestsFixture.RepositoryTestDouble;
    }

    [Fact]
    public async Task Test()
    {
        // Act
        var message = new AddAuditLog
        {
            EntityChange = "Create",
            EntityId = "124",
            EntityType = "Cover",
            TimeStamp = DateTime.UtcNow
        };
        await _messagesPublisher.PublishAsync(message);

        // Assert
        AuditLog? auditLog = null;
        for (var i = 0; i < 5 && auditLog is null; i++)
        {
            auditLog = _repositoryTestDouble.Get();
            await Task.Delay(1000);
        }

        auditLog.Should().NotBeNull();
        auditLog!.EntityChange.Should().BeEquivalentTo(message.EntityChange);
        auditLog.EntityId.Should().BeEquivalentTo(message.EntityId);
        auditLog.EntityType.Should().BeEquivalentTo(message.EntityType);
        auditLog.TimeStamp.Should().Be(message.TimeStamp);
    }
}
