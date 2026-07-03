# DiscogsClient

[![NuGet](https://img.shields.io/nuget/v/PolyhydraGames.DiscogsClient.svg)](https://www.nuget.org/packages/PolyhydraGames.DiscogsClient/)
[![License](https://img.shields.io/github/license/lancer1977/DiscogsClient.svg)](./LICENSE)

C# client library for [Discogs API v2.0](https://www.discogs.com/developers/).

The current package is `PolyhydraGames.DiscogsClient`. The library targets `net8.0`; the local test project targets `net10.0`.

## Tags

- discogs-client
- discogs
- dotnet
- auth
- docs
- client

## Features

- Include API to authorize user with OAuth 1.0 token and token secret.
- Full support to [Database API](https://www.discogs.com/developers/#page:database), including image download.
- Support of identity API.
- Transparent support of rate limit.
- Asynchronous and cancellable API using Tasks.
- Transparent management of pagination using non-blocking API (`IObservable`) or `IEnumerable`.

## Install

```bash
dotnet add package PolyhydraGames.DiscogsClient
```

## Sample Usage

### OAuth Authentication

```csharp
using DiscogsClient.RestHelpers.OAuth1;

var auth = new OAuthCompleteInformation(
    "consumerKey",
    "consumerSecret",
    "token",
    "tokenSecret");

var discogsClient = new DiscogsClient.DiscogsClient(auth);
```

### Token Authentication

```csharp
using DiscogsClient.Internal;

var tokenInformation = new TokenAuthenticationInformation("my-token");
var discogsClient = new DiscogsClient.DiscogsClient(tokenInformation);
```

### Search the Database

Using `IObservable`:

```csharp
using DiscogsClient.Data.Query;

var search = new DiscogsSearch
{
    artist = "Ornette Coleman",
    release_title = "The Shape Of Jazz To Come"
};

var observable = discogsClient.Search(search);
```

Using `IEnumerable`:

```csharp
var enumerable = discogsClient.SearchAsEnumerable(search);
```

### Get Release, Master, Artist, or Label Information

```csharp
var release = await discogsClient.GetReleaseAsync(1704673);
var master = await discogsClient.GetMasterAsync(47813);
var artist = await discogsClient.GetArtistAsync(224506);
var label = await discogsClient.GetLabelAsync(125);
```

### Download an Image

```csharp
var master = await discogsClient.GetMasterAsync(47813);

await discogsClient.SaveImageAsync(
    master.images[0],
    Path.GetTempPath(),
    "Ornette-TSOAJTC");
```

### Authorize a New User

```csharp
using DiscogsClient.RestHelpers.OAuth1;

var consumer = new OAuthConsumerInformation("consumerKey", "consumerSecret");
var authentifierClient = new DiscogsAuthentifierClient(consumer);

var oauth = await authentifierClient.Authorize(url => Task.FromResult(GetToken(url)));
```

`Authorize` takes a `Func<string, Task<string>>` parameter. It receives the Discogs authorization URL and returns the verifier value from the completed authorization flow.

```csharp
private static string GetToken(string url)
{
    Console.WriteLine("Authorize the application, then enter the verifier value.");
    Process.Start(url);
    return Console.ReadLine();
}
```

See [DiscogsClientTest](./DiscogsClient.Test/DiscogsClientTest.cs) and [DiscogsAuthenticationConsole](./DiscogsAuthenticationConsole/Program.cs) for fuller samples.

## Build, Test, and Pack

```bash
bash scripts/validate.sh
```

The normal test path is deterministic and does not require Discogs credentials. Live Discogs API tests are opt-in:

```bash
DISCOGS_LIVE_TESTS=true DISCOGS_TOKEN=<token> dotnet test DiscogsClient.Test/DiscogsClient.Test.csproj
```

GitHub Actions runs the canonical restore/test/pack path in `.github/workflows/package.yml`
and uploads the generated `.nupkg` as the `discogsclient-packages` workflow
artifact. Publishing defaults to GitHub Packages through the release trigger or
manual workflow dispatch with `publish=true`; it uses the repository
`GITHUB_TOKEN` package permission and does not require a committed secret.

NuGet.org publishing is not automated in this repository. Treat it as an
explicit maintainer action after the GitHub Packages workflow artifact has been
validated.

## Documentation

Detailed documentation can be found in the following sections:

- [Docs README](./docs/README.md)
- [Validation](./docs/validation.md)
- [Feature Index](./docs/features/README.md)
- [Core Capabilities](./docs/features/core-capabilities.md)
- [Roadmap Index](./docs/roadmaps/README.md)
