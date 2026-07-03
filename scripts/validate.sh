#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

solution="${DISCOGS_SOLUTION:-DiscogsClient.sln}"
configuration="${CONFIGURATION:-Release}"

echo "==> Running Api.Discogs validation"
dotnet --version
dotnet restore "$solution"
dotnet build "$solution" --configuration "$configuration" --no-restore
dotnet test "$solution" --configuration "$configuration" --no-restore --no-build --verbosity normal

echo "Validation passed."
