namespace DiscogsClient.Internal
{
    public class TokenAuthenticationInformation
    {
        private readonly string _SecretToken;

        public TokenAuthenticationInformation(string token)
        {
            Token = token;
            _SecretToken = $"Discogs token={token}";
        }

        public string Token { get; }

        public string GetDiscogsSecretToken()
        {
            return _SecretToken;
        }
    }
}