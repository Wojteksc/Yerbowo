namespace Yerbowo.Unit.Tests.Factories;

public static class HttpContextAccessorFactory
{
    public static HttpContextAccessor Create(Mock<ISession> sessionMock)
    {
        return new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext
            {
                Session = sessionMock.Object
            }
        };
    }
}