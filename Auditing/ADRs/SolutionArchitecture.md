# Solution Architecture

## Status
Approved

## Parties Involved
Przemyslaw Smolana

## Date
23/06/2024

## Context
It was agreed to keep Auditing context generic and to consume messages from RabbitMQ to keep other context independent. At the moment, after message is consumed, no data transformation will happen. The Auditing worker should be kept simple since it will not evolve in the future. Nevertheless, we should be ready in case, we would like to transform incoming data.

## Decision
Auditing worker will be implemented with only two layers. One for consuming message and the other for saving to the database.

## Consequences
It gives us freedom in the future, in case we would like to add some filtering or data transformation. It then can be Pipes and Filters architecture.

## Alternative
At this point, in time no other alternative was considered.
