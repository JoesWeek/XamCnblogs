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
    public class UserHttpClient : BaseHttpClient
    {
        static UserHttpClient baseHttpClient;
        public static UserHttpClient Current
        {
            get { return baseHttpClient ?? (baseHttpClient = new UserHttpClient()); }
        }
        readonly HttpClient client;
        public UserHttpClient()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(Apis.Host)
            };
            client.Timeout = TimeSpan.FromSeconds(10);
        }
        public async Task<ResponseMessage> GetAsyn(string url)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserTokenSettings.Current.UserTokenType, UserTokenSettings.Current.UserToken);
            var response = await client.GetAsync(url);
            return await GetResultMessage(response);
        }
        public async Task<ResponseMessage> PostAsync(string url, HttpContent content)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserTokenSettings.Current.UserTokenType, UserTokenSettings.Current.UserToken);
            var response = await client.PostAsync(url, content);
            return await GetResultMessage(response);
        }
        private async Task<ResponseMessage> CheckTokenAsync()
        {
            var message = new ResponseMessage();
            if (UserTokenSettings.Current.HasExpiresIn())
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
            //Settings.Current.AccessToken = token.AccessToken;
            //Settings.Current.ExpiresIn = token.ExpiresIn;
            //Settings.Current.TokenType = token.TokenType;
            //Settings.Current.TokenRefreshTime = DateTime.Now;
        }
    }
}
