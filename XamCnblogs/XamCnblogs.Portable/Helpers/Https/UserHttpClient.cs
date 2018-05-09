using Microsoft.AppCenter.Crashes;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class UserHttpClient : BaseHttpClient
    {
        private static readonly HttpClient client;

        static UserHttpClient()
        {
            client = new HttpClient(new NativeMessageHandler() { Timeout = new TimeSpan(0, 0, 60), EnableUntrustedCertificates = true, DisableCaching = true })
            {
                BaseAddress = new Uri(Apis.Host)
            };
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }
        static UserHttpClient baseHttpClient;
        public static UserHttpClient Current
        {
            get { return baseHttpClient ?? (baseHttpClient = new UserHttpClient()); }
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
        public async Task<ResponseMessage> DeleteAsync(string url)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserTokenSettings.Current.UserTokenType, UserTokenSettings.Current.UserToken);
            var response = await client.DeleteAsync(url);
            return await GetResultMessage(response);
        }
        public async Task<ResponseMessage> PatchAsync(string url, HttpContent content)
        {
            var responseMessage = new ResponseMessage();
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserTokenSettings.Current.UserTokenType, UserTokenSettings.Current.UserToken);

                var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = content
                });
                responseMessage = await GetResultMessage(response);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                responseMessage = new ResponseMessage() { Success = false, Message = ex.Message };
            }
            return responseMessage;
        }
        public async Task<ResponseMessage> RefreshTokenAsync()
        {
            var responseMessage = new ResponseMessage();
            try
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("grant_type", "refresh_token");
                parameters.Add("refresh_token", UserTokenSettings.Current.UserRefreshToken);
                var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":" + ClientSercret));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basic);
                var response = await client.PostAsync(Apis.Token, new FormUrlEncodedContent(parameters));
                responseMessage = await GetResultMessage(response);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                responseMessage = new ResponseMessage() { Success = false, Message = ex.Message };
            }
            return responseMessage;
        }
        private async Task<ResponseMessage> GetResultMessage(HttpResponseMessage response)
        {
            var code = response.StatusCode;
            switch (code)
            {
                case HttpStatusCode.OK:
                    return new ResponseMessage() { Success = true, Code = HttpStatusCode.OK, Message = await response.Content.ReadAsStringAsync() };
                case HttpStatusCode.Created:
                    return new ResponseMessage() { Success = true, Code = HttpStatusCode.Created, Message = await response.Content.ReadAsStringAsync() };
                default:
                    var message = await response.Content.ReadAsStringAsync();
                    try
                    {
                        Crashes.TrackError(new Exception() { Source = message });
                        message = JsonConvert.DeserializeObject<Messages>(await response.Content.ReadAsStringAsync()).Message;
                    }
                    catch (Exception e)
                    {
                        message = response.StatusCode.ToString();
                    }
                    return new ResponseMessage() { Success = false, Code = response.StatusCode, Message = message };
            }
        }
    }
    public class Messages
    {
        public string Message { get; set; }
    }
}
