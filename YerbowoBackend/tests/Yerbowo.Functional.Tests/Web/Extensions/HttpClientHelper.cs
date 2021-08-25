using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yerbowo.Functional.Tests.Web.Extensions
{
    public static class HttpClientHelper
    {
        public static async Task<TReturn> GetAsync<TReturn>(this HttpClient httpClient, string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri);

            VerifyResponse(response);

            return await DeserializeObject<TReturn>(response);
        }

        public static async Task<HttpResponseMessage> PutAsync<TBody>(this HttpClient httpClient, string requestUri, TBody body)
        {
            HttpRequestMessage requestMessage = CreateRequestMessage(body);

            var response = await httpClient.PutAsync(requestUri, requestMessage.Content);

            VerifyResponse(response);

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync<TBody>(this HttpClient httpClient, string requestUri, TBody body)
        {
            var requestMessage = CreateRequestMessage(body);

            var response = await httpClient.PostAsync(requestUri, requestMessage.Content);

            VerifyResponse(response);

            return response;
        }

        public static async Task<TReturn> PostAsync<TBody, TReturn>(this HttpClient httpClient, string requestUri, TBody body)
        {
            var requestMessage = CreateRequestMessage(body);

            var response = await httpClient.PostAsync(requestUri, requestMessage.Content);

            VerifyResponse(response);

            return await DeserializeObject<TReturn>(response);
        }

        private static HttpRequestMessage CreateRequestMessage<TBody>(TBody body)
        {
            return new HttpRequestMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };
        }

        private static void VerifyResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.StatusCode.ToString());
        }

        private static async Task<TReturn> DeserializeObject<TReturn>(HttpResponseMessage httpResponseMessage)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturn>(responseString);
        }
    }
}
