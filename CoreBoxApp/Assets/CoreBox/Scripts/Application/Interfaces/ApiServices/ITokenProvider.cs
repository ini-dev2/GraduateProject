namespace CoreBox
{
    internal interface ITokenProvider
    {
        public string Token { get; }
        public void SaveToken(string token);
    }
}