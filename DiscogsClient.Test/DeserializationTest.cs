namespace DiscogsClient.Test
{
    public abstract class DeserializationTest<T>
    {
        protected abstract string JSON { get; }
        protected T Result { get; }

        protected DeserializationTest()
        {
            Result = JsonSerializer.Deserialize<T>(JSON);
        }
    }
}