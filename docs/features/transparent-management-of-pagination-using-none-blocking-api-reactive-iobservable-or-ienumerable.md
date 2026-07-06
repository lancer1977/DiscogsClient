---
title: Transparent management of pagination using none blocking API (Reactive IObservable) or IEnumerable
status: done
owner: @DreadBreadcrumb
priority: high
complexity: 1
created: 2026-03-22
updated: 2026-03-22
tags: [feature, DiscogsClient, existing]
---

# Transparent management of pagination using none blocking API (Reactive IObservable) or IEnumerable

Existing implementation of Transparent management of pagination using none blocking API (Reactive IObservable) or IEnumerable within the DiscogsClient codebase.

## Behavior

- Paginable request parameters use Discogs `page` and `per_page` query names.
- Enumerable and observable search paths request subsequent pages until the requested max result count is satisfied or the API reports the last page.
- The `max` argument lowers `per_page` for the generated paginable request when fewer than the Discogs page maximum is needed.

## Validation

- `RequestShapeTest.Paginable_parameters_use_discogs_page_names`
- `DiscogsClientBehaviorTest.SearchAsEnumerable_RequestsPagesUntilMaxResultsAreSatisfied`
