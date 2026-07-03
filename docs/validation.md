# Validation

Run the repo-native validation gate from the repository root:

```bash
bash scripts/validate.sh
```

The gate restores, builds, and tests `DiscogsClient.sln` in `Release` configuration. Override the target solution or configuration only for local diagnostics:

```bash
DISCOGS_SOLUTION=DiscogsClient.sln CONFIGURATION=Release bash scripts/validate.sh
```

## Credential Boundary

Automated validation is secret-free and does not require live Discogs credentials.

Manual OAuth and auth-console checks may use the optional variables documented in [`.env.example`](../.env.example):

- `DISCOGS_CONSUMER_KEY`
- `DISCOGS_CONSUMER_SECRET`
- `DISCOGS_TOKEN`
- `DISCOGS_TOKEN_SECRET`

Keep real values in your shell, user secrets, or another local-only secret store.
