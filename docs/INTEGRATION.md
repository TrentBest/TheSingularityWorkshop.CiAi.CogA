# CogA integration note

**Status: proposal only — not a finalized shared wire contract.**

CogA proposes an offline typed catalog that CiAi can later use as an input to host-controlled routing and policy checks. This implementation must not be interpreted as permission to dispatch or as evidence that a repository or workflow exists.

## Proposed responsibilities

- **CogA:** repository identity, capability definitions/declarations, directed relationships, structural validation.
- **CiAi:** reconcile the catalog with trusted repository metadata and host policy; remain responsible for orchestration and authorization.
- **CogB:** define outbound request envelope and transport abstraction.
- **CogC:** define result envelope, correlation, and evidence validation.
- **CogD:** reconcile controlled operation vocabulary with the actual ProtocolAi API.
- **CogE:** consume reconciled topology/evidence in a presentation-neutral node/edge model.
- **GitHub:** authoritative source for repository metadata, permissions, commit SHAs, workflow state, and run evidence.

## Contract questions requiring reconciliation

1. Is `owner/name` the agreed canonical repository key, and what normalization rules apply?
2. Which capability and relationship identifiers will ProtocolAi own or map?
3. Which contract-version grammar and compatibility policy should be shared across request/result envelopes?
4. How should authorization requirements reference host policy without implying that a catalog grants permission?
5. What evidence identity fields must be linked to topology edges once workflow observation is integrated?

## Non-goals

This repository will not edit sibling repositories, call GitHub, dispatch workflows, manage credentials, publish packages, or render GUI. The initial model is an offline proposal. Any cross-repository write or live dispatch requires explicit owner approval and a separate tested integration path.
