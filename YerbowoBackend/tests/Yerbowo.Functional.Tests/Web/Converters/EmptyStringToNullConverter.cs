namespace Yerbowo.Functional.Tests.Web.Converters;

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
        string value = reader.GetString();
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
