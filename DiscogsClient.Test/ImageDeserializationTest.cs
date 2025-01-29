namespace DiscogsClient.Test;

public class ImageDeserializationTest : DeserializationTest<DiscogsImage>
{
    protected override string JSON => "{ \"height\": 569, \"resource_url\": \"https://api-img.discogs.com/_0K5t_iLs6CzLPKTB4mwHVI3Vy0=/fit-in/600x569/filters:strip_icc():format(jpeg):mode_rgb():quality(96)/discogs-images/R-66785-1213949871.jpeg.jpg\", \"type\": \"primary\", \"uri\": \"https://api-img.discogs.com/_0K5t_iLs6CzLPKTB4mwHVI3Vy0=/fit-in/600x569/filters:strip_icc():format(jpeg):mode_rgb():quality(96)/discogs-images/R-66785-1213949871.jpeg.jpg\", \"uri150\": \"https://api-img.discogs.com/sSWjXKczZseDjX2QohG1Lc77F-w=/fit-in/150x150/filters:strip_icc():format(jpeg):mode_rgb()/discogs-images/R-66785-1213949871.jpeg.jpg\", \"width\": 600}";


    [Fact]
    public void DeserializeResult_IsNotNull()
    {
        Result.Should().NotBeNull();
    }

    [Fact]
    public void DeserializeHeight_IsOK()
    {
        Result.height.Should().Be(569);
    }

    [Fact]
    public void DeserializeWidth_IsCorrect()
    {
        Result.width.Should().Be(600);
    }

    [Fact]
    public void DeserializeResource_url_IsCorrect()
    {
        Result.resource_url.Should().Be(@"https://api-img.discogs.com/_0K5t_iLs6CzLPKTB4mwHVI3Vy0=/fit-in/600x569/filters:strip_icc():format(jpeg):mode_rgb():quality(96)/discogs-images/R-66785-1213949871.jpeg.jpg");
    }

    [Fact]
    public void Deserializetype_HasCorrectValue()
    {
        Result.type.Should().Be(DiscogsImageType.primary);
    }
}