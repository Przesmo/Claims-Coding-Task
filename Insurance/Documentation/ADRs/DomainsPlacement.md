# Domains Placement

## Status
Approved

## Parties Involved
Przemyslaw Smolana

## Date
23/06/2024

## Context
Main question is if Covers and Claims should be single running instances or should be kept in the same instance.
Insurance context is owned by the same team. At the moment, it is a bit unknown how they will evolve in the future.

## Decision
Decided to keep both domains in the single running instance.

## Consequences
It can cause a trouble in case when some change in Covers module will be introduced and it may break Claims functionality too. However, if we would recognize that Covers module needs to be run as separate instance, it won't be a problem since we keep functionality apart.

## Alternative
We can run both domains separate from the beginning. However, we don't see much benefits of having it or rather a burden due to supporting two running instances.
