namespace DiscogsClient.Test
{
    public abstract class DeserializationTest<T>
    {
        protected DeserializationTest()
        {
            Result = JsonSerializer.Deserialize<T>(JSON);
        }

        protected abstract string JSON { get; }
        protected T Result { get; }
    }
}