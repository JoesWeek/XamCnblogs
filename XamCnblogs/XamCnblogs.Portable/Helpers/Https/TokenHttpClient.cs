using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class BaseHttpClient
    {
        public const string ClientId = "cda7b086-4cdf-4aaf-bde5-2aefabefa828";
        public const string ClientSercret = "E9xR1T6fMb8WJ8fqr3AXMuSrAHUfD4Tgo6MxlArBF5o_XxVK9IWmC498PyM03aAZILhhYHTwgszFFmAk";

    }
    public class TokenHttpClient : BaseHttpClient
    {
        static TokenHttpClient baseHttpClient;
        public static TokenHttpClient Current
        {
            get { return baseHttpClient ?? (baseHttpClient = new TokenHttpClient()); }
        }
        readonly HttpClient client;
        public TokenHttpClient()
        {
            client = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(Apis.Host)
            };
            client.Timeout = TimeSpan.FromSeconds(10);
        }
        public async Task<ResponseMessage> GetAsyn(string url)
        {
            var result = CheckTokenAsync().Result;
            if (result.Success)
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AccessTokenSettings.Current.TokenType, AccessTokenSettings.Current.AccessToken);
                    var response = await client.GetAsync(url);
                    return await GetResultMessage(response);
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = ex.Message;
                    return result;
                }
            }
            else
            {
                return result;
            }
        }
        public async Task<ResponseMessage> PostTokenAsync(string code)
        {
            var grant_type = "authorization_code";
            var redirect_uri = "https://oauth.cnblogs.com/auth/callback";

            var content = string.Format("client_id={0}&client_secret={1}&grant_type={2}&redirect_uri={3}&code={4}", ClientId, ClientSercret, grant_type, redirect_uri, code);

            var response = await client.PostAsync(Apis.Token, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
            return await GetResultMessage(response);
        }
        private async Task<ResponseMessage> CheckTokenAsync()
        {
            var message = new ResponseMessage();
            if (AccessTokenSettings.Current.AccessTokenExpiresIn)
            {
                try
                {
                    var result = await TokenAsync();
                    if (result.Success)
                    {
                        UpdateToken(JsonConvert.DeserializeObject<Token>(result.Message.ToString()));
                    }
                    else
                    {
                        result.Success = false;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    message.Success = false;
                    message.Message = ex.Message;

                    return message;
                }
            }
            else
            {
                message.Success = true;
            }
            return message;
        }
        private async Task<ResponseMessage> TokenAsync()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":" + ClientSercret));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basic);
            var response = await client.PostAsync(Apis.Token, new FormUrlEncodedContent(parameters));
            return await GetResultMessage(response);
        }
        private async Task<ResponseMessage> GetResultMessage(HttpResponseMessage response)
        {
            var code = response.StatusCode;
            switch (code)
            {
                case HttpStatusCode.OK:
                    return new ResponseMessage() { Success = true, Message = await response.Content.ReadAsStringAsync() };
                default:
                    return new ResponseMessage() { Success = false, Message = response.StatusCode };
            }
        }
        private void UpdateToken(Token token)
        {
            AccessTokenSettings.Current.AccessToken = token.AccessToken;
            AccessTokenSettings.Current.ExpiresIn = token.ExpiresIn;
            AccessTokenSettings.Current.TokenType = token.TokenType;
            AccessTokenSettings.Current.TokenRefreshTime = DateTime.Now;
        }
    }
}
