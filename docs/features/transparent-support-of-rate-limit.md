---
title: Transparent support of rate limit
status: done
owner: @DreadBreadcrumb
priority: high
complexity: 1
created: 2026-03-22
updated: 2026-03-22
tags: [feature, DiscogsClient, existing]
---

# Transparent support of rate limit

Existing implementation of Transparent support of rate limit within the DiscogsClient codebase.

## Behavior

- Discogs web clients share a single rate limiter for API calls.
- Request execution goes through the rate-limited `RestSharpWebClient` path.
- Web-client failures propagate through the public enumeration path instead of being silently converted into partial results.

## Validation

- `RequestShapeTest.Discogs_web_clients_share_the_same_rate_limiter`
- `DiscogsClientBehaviorTest.SearchAsEnumerable_WhenClientFails_PropagatesClientError`
