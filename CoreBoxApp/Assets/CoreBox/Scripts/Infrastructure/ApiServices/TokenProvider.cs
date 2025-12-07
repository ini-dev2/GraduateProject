using UnityEngine;

namespace CoreBox
{
    internal sealed class TokenProvider : ITokenProvider
    {
        private string token;
        public string Token => token;

        public void SaveToken(string token)
        {
            if (token == null || string.IsNullOrEmpty(token))
            {
                Debug.LogError("Token is null");
                return;
            }
            this.token = token;
        }
    }
}