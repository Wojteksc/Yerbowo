namespace Yerbowo.Unit.Tests.Helpers;

public static class SerializerHelper
{
    public static byte[] SerializeObjectToBytes(object testObject)
        => Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(testObject));
}