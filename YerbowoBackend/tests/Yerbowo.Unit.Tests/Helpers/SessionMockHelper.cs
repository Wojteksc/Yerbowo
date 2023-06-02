namespace Yerbowo.Unit.Tests.Helpers;

public static class SessionMockHelper
{
    public static Mock<ISession> SetupSession(string key = null, object testObject = null)
    {
        var sessionMock = new Mock<ISession>();

        if (testObject != null && key != null)
        {
            var value = SerializerHelper.SerializeObjectToBytes(testObject);
            sessionMock.Setup(s => s.Set(key, It.IsAny<byte[]>())).Callback<string, byte[]>((k, v) => value = v);
            sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);
        }

        return sessionMock;
    }
}