# Validation

Run the repo-native validation gate from the repository root:

```bash
bash scripts/validate.sh
```

The gate restores `PolyhydraGames.Discogs.sln`, runs the release test path, and packs `PolyhydraGames.DiscogsClient`.

## Credential Boundary

Default validation is deterministic and does not require Discogs credentials. Live Discogs API tests are opt-in and should use local-only values:

```bash
DISCOGS_LIVE_TESTS=true DISCOGS_TOKEN=<token> bash scripts/validate.sh
```

Optional variable names are listed in [`.env.example`](../.env.example). Do not commit real tokens or OAuth secrets.

## Artifacts

Package output is written to `artifacts/packages` by default. Override `PACKAGE_OUTPUT` for local diagnostics when you do not want package artifacts under the repo root.
