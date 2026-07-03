# Api.Discogs

**Location:** `~/code/APIs/Api.Discogs`

**Purpose:** Discogs API client + auth console.

## Validation

Use the repo-native validation gate:

```bash
bash scripts/validate.sh
```

Live Discogs OAuth credentials are not required for automated validation.

## Projects

| Project | Files | Purpose |
|---------|-------|---------|
| `DiscogsClient` | 63 | Main Discogs REST client |
| `RestSharpHelper` | 12 | RestSharp helpers (Newtonsoft.Json) |
| `DiscogsAuthenticationConsole` | 2 | CLI tool for auth/token flow |
| `DiscogsClient.Test` | 10 | Tests |

## Dependencies

- `RestSharp`
- `RateLimiter`
- `System.Reactive`
- `Newtonsoft.Json`

## Status

✅ **Working**
