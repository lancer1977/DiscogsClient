# Test Gaps

## Covered

- Identity request shape and per-client identity caching have deterministic tests.
- Pagination query shape and multi-page enumerable behavior have deterministic tests.
- Rate-limit plumbing has a shared-limiter assertion, and the public enumerable path has error propagation coverage.
- Existing fixture tests cover result deserialization for search, artist, release, master, image, track, and version payloads.

## Gaps Still Worth Watching

- Live Discogs API behavior remains opt-in because it requires credentials and network access.
- Rate-limit timing is not tested with sleeps; deterministic coverage asserts shared limiter use instead.
- OAuth browser authorization remains sample-driven rather than covered by an end-to-end automated credential flow.
