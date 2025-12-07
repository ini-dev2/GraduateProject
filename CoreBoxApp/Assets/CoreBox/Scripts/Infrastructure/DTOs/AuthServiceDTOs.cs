using Newtonsoft.Json;
using System;

namespace CoreBox
{
    /// <summary>
    /// DTO for entering the application
    /// </summary>
    internal sealed class LoginDTO
    {
        [JsonProperty("email")]
        public string Email;

        [JsonProperty("password")]
        public string Password;

        public LoginDTO()
        {
        }

        public LoginDTO(string email, string password)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }

    /// <summary>
    /// DTO for registering in the application
    /// </summary>
    internal sealed class RegisterDTO
    {
        [JsonProperty("username")]
        public string UserName;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("password")]
        public string Password;

        public RegisterDTO()
        {
        }

        public RegisterDTO(string userName, string email, string password)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
    
    /// <summary>
    /// DTO after request
    /// </summary>
    internal sealed class AuthResponseDTO : BaseStatusDTO
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("username")]
        public string UserName;

        [JsonProperty("role")]
        public string Role;

        public AuthResponseDTO()
        {
        }

        public AuthResponseDTO(bool isSuccess, string statusMessage) : base(isSuccess, statusMessage)
        {
        }

        public AuthResponseDTO(string token, string userName, string role, bool isSuccess, string statusMessage) : base(isSuccess, statusMessage)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}