using Newtonsoft.Json;
using System;

namespace XamCnblogs.Portable.Model
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("IsIdentityUser")]
        public bool IsIdentityUser { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTime { get; set; }
    }
}
