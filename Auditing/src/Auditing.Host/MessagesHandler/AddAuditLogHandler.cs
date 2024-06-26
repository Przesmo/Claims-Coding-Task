﻿using Auditing.Host.Contracts;
using Auditing.Host.Repositories;
using EasyNetQ;

namespace Auditing.Host.MessagesHandler;

public class AddAuditLogHandler : IAddAuditLogHandler
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AddAuditLogHandler(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    //Depending on the actual business case some message validation can be done
    public async Task HandleAsync(AddAuditLog messageBody, MessageProperties messageProperties,
        CancellationToken cancellationToken = default) => await _auditLogRepository.AddAsync(
            new AuditLog
            {
                EntityId = messageBody.EntityId,
                EntityChange = messageBody.EntityChange,
                EntityType = messageBody.EntityType,
                TimeStamp = messageBody.TimeStamp
            }, cancellationToken);
}
