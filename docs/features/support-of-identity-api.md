---
title: Support of identity API
status: done
owner: @DreadBreadcrumb
priority: high
complexity: 1
created: 2026-03-22
updated: 2026-03-22
tags: [feature, DiscogsClient, existing]
---

# Support of identity API

Existing implementation of Support of identity API within the DiscogsClient codebase.

## Behavior

- `GetUserIdentityAsync` targets the Discogs `oauth/identity` resource.
- The first successful identity response is cached by the client instance.
- Release-rating writes and deletes use the cached identity username to address user-scoped rating endpoints.

## Validation

- `DiscogsClientBehaviorTest.GetUserIdentityAsync_CachesIdentityAfterFirstRequest`
- `RequestShapeTest.Identity_request_targets_oauth_identity_resource`
- `RequestShapeTest.User_release_rating_requests_use_username_and_release_segments`
