namespace Yerbowo.Application.Services.WebEncoder;

[ExcludeFromCodeCoverage]
public class WebEncoder : IWebEncoder
{
    public string Base64UrlEncodeGuid()
    {
        return WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());
    }
}