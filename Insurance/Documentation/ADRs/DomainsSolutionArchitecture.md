# Domains Solution Architecture

## Status
Approved

## Parties Involved
Przemyslaw Smolana

## Date
23/06/2024

## Context
The ADR is for two sub-domains (Covers and Claims) since we recognized both have the same characteristics.
Domains are owned by the same team. Covers and Claims have simple validation and doesn't require introducing deep/domain model at the moment.
Simple CRUD operation should be supported. In most cases only one user is accessing/modyfing resource. All data are saved to SQL database. Currently, no need for separate presentation model.

## Decision
Decided to implement 3-layer architecture.

## Consequences
The solution is kept simple to the minimum. It should give us flexible to extend functionality. Nevertheless, if more dependencies will be introduced or more complex domain validation will be required, with current architecture style, team could have a problem with implementing new functionalties.

## Alternative
We could introduce domain model that would protect invariants. However, validation is rather simple and we are not considering concurrent access to the model.
