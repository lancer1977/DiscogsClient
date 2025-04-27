
using System;

namespace DiscogsClient.Test;

public class TrackDeserializationTest : DeserializationTest<DiscogsTrack>
{
    protected override string JSON => "{\"duration\": \"7:13\",\"position\": \"2\",\"type_\": \"track\",\"extraartists\": [  {\"join\": \"\",\"name\": \"DJ Sangeet\",\"anv\": \"\",\"tracks\": \"\",\"role\": \"Written-By, Producer\",\"resource_url\": \"https://api.discogs.com/ReleaseArtists/25460\",\"id\": 25460  }],\"title\": \"From The Heart\"}";



    [Fact]
    public void DeserializeResult_IsNotNull()
    {
        Result.Should().NotBeNull();
    }

    [Fact]
    public void DeserializeDuration_IsOK()
    {
        Result.duration.Should().Be(new TimeSpan(0, 7, 13));
    }

    [Fact]
    public void DeserializePosition_IsCorrect()
    {
        Result.position.Should().Be("2");
    }

    [Fact]
    public void DeserializeTitle_IsCorrect()
    {
        Result.title.Should().Be("From The Heart");
    }

    [Fact]
    public void DeserializeExtraArtists_HasCorrectSize()
    {
        Result.extraartists.Should().NotBeNull();
        Result.extraartists.Should().HaveCount(1);
    }

    [Fact]
    public void DeserializeExtraArtists_HasCorrectInformation()
    {
        Result.extraartists[0].name.Should().Be("DJ Sangeet");
        Result.extraartists[0].resource_url.Should().Be("https://api.discogs.com/ReleaseArtists/25460");
    }
}