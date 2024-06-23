# Auditing Context Relationship

## Status
Approved

## Parties Involved
Przemyslaw Smolana

## Date
23/06/2024

## Context
Due to regulations each change in the system has to be logged. Therefore in the future, we are able to provide full audit of all changes. It means that each context (including new one) has to comunicate with Auditing context to save their changes.
Taking it into account, we have to define direction of dependency between Auditing context and other contexts. Key architecture driver is modules autonomy.

## Decision
Auditing context will be Open Host Service. Communication will happen asynchronous via RabbitMQ. Each context will have to publish generic message - command to save audit details. Command will be defined by Auditing context.

## Consequences
The relationship between them will be Upstream (Auditing context) and Downstream (other contexts). Other contexts will depend on Auditing context. It means, whenever change happen in the contract on Auditing context side, other contexts will have to adapt to it. However, taking into account that Auditing context is generic domain - therefore contract should remain stable.

## Alternative
Potentially, we can inverse dependency so Auditing context will depend on other contexts. It could lead to not meet main criteria of modules autonomy. Auditing context would have to adapt to individual changes in each context. The consequences of it will be Auditing context being main bottleneck of a release. Since each team responsible for individual module would have to wait until changes in Auditing context are done.
