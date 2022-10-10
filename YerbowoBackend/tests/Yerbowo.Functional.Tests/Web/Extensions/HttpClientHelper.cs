using System.Reflection;
using System.Text.Json.Serialization;

namespace Yerbowo.Functional.Tests.Web.Extensions;

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
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };
    }

    private static void VerifyResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.StatusCode.ToString());
    }

    private static async Task<TReturn> DeserializeObject<TReturn>(HttpResponseMessage httpResponseMessage)
    { 
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new EmptyStringToNullConverter() }
        };

        string responseString = await httpResponseMessage.Content.ReadAsStringAsync();

        if(httpResponseMessage.StatusCode == HttpStatusCode.NoContent && 
            string.IsNullOrEmpty(responseString))
        {
            return default(TReturn);
        }

        return JsonSerializer.Deserialize<TReturn>(responseString, options);
    }
}

/// <summary>
/// Convert empty to null when read data json
/// </summary>
public class EmptyStringToNullConverter : JsonConverter<string>
{
    /// <summary>
    /// Override CanConvert method of JsonConverter
    /// This instance only convert the string type.
    /// </summary>
    /// <returns></returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }

    /// <summary>
    /// Override ReadJson method of JsonConverter
    /// Convert string null to empty
    /// </summary>
    /// <returns></returns>
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = (string)reader.GetString();
        return value ?? String.Empty;
    }

    /// <summary>
    /// Override WriteJson method of JsonConverter
    /// </summary>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Unnecessary");
    }
}