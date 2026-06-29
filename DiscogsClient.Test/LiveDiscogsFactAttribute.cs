using System;

namespace DiscogsClient.Test;

public sealed class LiveDiscogsFactAttribute : FactAttribute
{
    private const string EnabledVariable = "DISCOGS_LIVE_TESTS";
    private const string TokenVariable = "DISCOGS_TOKEN";

    public LiveDiscogsFactAttribute()
    {
        var enabled = string.Equals(Environment.GetEnvironmentVariable(EnabledVariable), "true",
            StringComparison.OrdinalIgnoreCase);
        var hasToken = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(TokenVariable));

        if (!enabled || !hasToken)
        {
            Skip = $"Set {EnabledVariable}=true and {TokenVariable} to run live Discogs integration tests.";
        }
    }
}
