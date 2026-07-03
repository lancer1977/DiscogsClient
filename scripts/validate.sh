#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

solution="${DISCOGS_SOLUTION:-PolyhydraGames.Discogs.sln}"
test_project="${DISCOGS_TEST_PROJECT:-DiscogsClient.Test/DiscogsClient.Test.csproj}"
package_project="${DISCOGS_PACKAGE_PROJECT:-DiscogsClient/PolyhydraGames.Discogs.csproj}"
configuration="${CONFIGURATION:-Release}"
package_output="${PACKAGE_OUTPUT:-artifacts/packages}"

echo "==> Running DiscogsClient validation"
dotnet --version
dotnet restore "$solution"
dotnet test "$test_project" --configuration "$configuration" --no-restore -v:minimal
dotnet pack "$package_project" --configuration "$configuration" --no-restore --output "$package_output"

echo "Validation passed."
