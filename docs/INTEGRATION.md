# CogA integration note

**Status: proposal only — not a finalized shared wire contract.**

CogA proposes an offline typed catalog that CiAi can later use as an input to host-controlled routing and policy checks. This implementation must not be interpreted as permission to dispatch or as evidence that a repository or workflow exists.

## Reconciled responsibility boundary

The parent CiAi contract at `docs/INTER_REPOSITORY_MODEL.md` and assignments at `docs/COG_ASSIGNMENTS.md` establish the following boundaries:

- **CogA:** repository identity, capability definitions/declarations, directed relationships, structural validation.
- **CiAi:** reconcile catalog claims with trusted repository metadata and host policy; own orchestration and authorization.
- **CogB:** outbound request envelope and transport abstraction.
- **CogC:** result envelope, correlation, and evidence validation.
- **CogD:** deterministic operation vocabulary and policy using the actual ProtocolAi API.
- **CogE:** presentation-neutral topology/evidence model.
- **GitHub:** authoritative repository metadata, permissions, commit SHAs, workflow state, and run evidence.

## Current model compared with the parent vocabulary

| Parent contract concept | CogA's current representation | Status |
| --- | --- | --- |
| Canonical repository identity | `RepositoryId`, represented as `owner/name` | Shape checked offline only; existence and authoritative identity are not verified. Case/normalization policy still needs agreement. |
| Composable repository capabilities | `CapabilityRole` and `CapabilityDeclaration` | Conceptually aligned. Names differ: e.g. `ExecutesBuilds` corresponds to “Build executor,” and `DefinesPolicy` to “Policy authority.” This is a mapping proposal, not a shared identifier registry. |
| Directed relationship types | `Provides`, `Consumes`, `Requests`, `Notifies`, `ReportsTo` | Same conceptual set as the parent contract; identifiers and serialized representation are not yet standardized. |
| Contract compatibility | `major.minor.patch` strings; current validator requires matching major versions | Deliberately narrower than a full semantic-version range policy. No range negotiation is implemented. |
| Authorization | Optional named `AuthorizationRequirement` label | Descriptive metadata only. CiAi must make the actual policy decision; a catalog entry never grants permission. |
| Correlation, idempotency, and run evidence | Not modeled by CogA's offline catalog | Deferred to the request/result integration led by CogB/CogC and authoritative GitHub evidence. Do not add these as if CogA alone can validate them. |
| Controlled vocabulary | Current capability IDs are application-owned strings | ProtocolAi mapping/ownership remains unresolved; inspect its real API and reconcile through CogD before treating IDs as shared protocol symbols. |

## ProtocolAi mapping proposal for reconciliation

ProtocolAi's documented model assigns symbols within a named protocol: a symbol has an integer identity and a name/value, and a reference is scoped by both protocol ID and symbol ID. Therefore, a CogA string such as `Requests` or `ExecutesBuilds` must **not** be assumed to be a ProtocolAi symbol ID, and a bare string must not be treated as globally unique semantic identity.

Recommended boundary, subject to CogD and parent-contract approval:

1. **Keep CogA's current string IDs as local catalog keys for this offline slice.** Do not add a ProtocolAi package dependency, hard-code numeric IDs, or imply that these strings are already wire-compatible.
2. **Have CogD own the controlled vocabulary proposal.** It should define the protocol identity and explicit symbols for approved relationship types, capability roles, and operation names using the actual ProtocolAi API. Numeric protocol/symbol IDs must come from that reviewed definition, not be guessed by CogA.
3. **Keep domain capability identity distinct from a generic role/type.** `ExecutesBuilds` describes a role; it does not identify the particular capability being provided. A capability ID needs an agreed namespace and version policy before it can be mapped safely.
4. **Translate at an explicit integration boundary.** CiAi can resolve a validated catalog key to an approved ProtocolAi reference after confirming the expected protocol identity. Unknown, wrong-protocol, or unmapped symbols must be rejected for action; a literal string is not authorization.
5. **Preserve source meaning and evidence.** Any future translated representation should retain the original catalog key and vocabulary/version provenance so mapping changes are reviewable. Do not silently renumber or reinterpret existing catalog entries.

This is a proposal for a seam between the independently tested cogs, not a claim that CogA can currently encode or resolve ProtocolAi payloads. It does not define a wire format or authorize dispatch.

## Contract questions requiring reconciliation

1. Confirm `owner/name` as the canonical key and decide case/normalization rules.
2. Have CogD propose the ProtocolAi protocol identity and symbol mapping; agree ownership, stable identifiers, and capability namespace before adopting shared IDs.
3. Agree on version grammar and compatibility semantics across catalog, request, and result models.
4. Define how authorization labels reference host policy without implying that a catalog grants permission.
5. Define which evidence fields link a topology edge to a correlated request/result and verified workflow run.
6. Decide whether the current strict self-edge rejection is a parent-contract invariant or only a CogA-local rule.

## Recommended next step

Review the mapping proposal with the parent contract and CogD before changing public model types or introducing a serialized schema. Keep CogA dependency-free and offline; let CiAi own trusted metadata resolution and authorization, CogB/CogC own request/result transport and evidence, CogD own deterministic vocabulary/policy alignment, and CogE own presentation-neutral projection.

## Non-goals

This repository will not edit sibling repositories, call GitHub, dispatch workflows, manage credentials, publish packages, or render GUI. The initial model is an offline proposal. Any cross-repository write or live dispatch requires explicit owner approval and a separate tested integration path.
