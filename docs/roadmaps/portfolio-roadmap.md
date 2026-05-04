# DiscogsClient portfolio roadmap

## 90-day evidence snapshot
- Commits (90 days): 10
- Files changed (90 days): 93
- Last signal: `023a7a4` (2 days ago)
- Top modified areas: `DiscogsClient` (64), `docs` (11), `DiscogsClient.Test` (10)
- Snapshot date: 2026-05-01

## Current posture
- Stack: .NET
- Docs folder: yes
- Roadmap folder: no
- Features docs: yes
- Tests indexed: yes (`DiscogsClient.Test`)
- Direction:
  - API client and pagination/rate-limit features are actively evolving
  - OAuth and identity support has notable recent relevance

## Discovery
- [x] Capture and timestamp recent change signal
- [x] Capture top-level area concentration
- [ ] Map API feature ownership to a release owner and owner for rate-limit/error handling
- [ ] Add explicit acceptance checks for OAuth and identity paths

## V1 (stability)
- [ ] Lock API request/response contract for major endpoints touched in recent changes
- [ ] Add deterministic smoke checks for OAuth/token and pagination behavior
- [ ] Expand docs for required environment/config and failure modes
- [ ] Add reproducible test/run instructions for test suite subset likely changed in last cycle

## V2 (confidence)
- [ ] Improve coverage and assertions for identity, pagination, and rate-limit behavior
- [ ] Add contract-oriented validation for API model changes
- [ ] Document operational runbook for token expiry and renewal
- [ ] Standardize feature checklists for support/behavior notes in `docs/features`

## V10 (scale)
- [ ] Introduce explicit API versioning policy for external consumers
- [ ] Add long-range compatibility matrix for breaking response shape changes
- [ ] Add resilience and retry policy documentation for high-volume usage
- [ ] Publish a roadmap of feature migration for newer Discogs endpoints

## Feature-to-roadmap mapping
- [x] Core client behavior: `docs/features/discogsclient.md`
- [x] API capabilities: `docs/features/core-capabilities.md`
- [x] OAuth/token behavior: `docs/features/include-api-to-authorize-user-generating-oauth1-0-token-and-token-secret.md`
- [x] Pagination/rate limits: `docs/features/transparent-management-of-pagination-using-none-blocking-api-reactive-iobservable-or-ienumerable.md`, `docs/features/transparent-support-of-rate-limit.md`
- [x] In-progress changes: `docs/features/in-progress-work.md`

## Release readiness checklist
- [ ] API contract and auth checks pass in one clean run
- [ ] Test suite path in docs is reproducible
- [ ] Changelog entry captures breaking or behavioral change
- [ ] Rollback path documented for token-related regressions

## Next move
Use V1 to lock API and identity path behavior, then V2/V10 can focus on versioning and high-volume reliability.
