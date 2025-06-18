namespace Yerbowo.Unit.Tests.Helpers;

public static class HttpContextAccessorMockHelper
{
    public static void SetupDefaultHttpContext(this Mock<IHttpContextAccessor> httpContextAccessor) 
        => httpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

    public static void ContainsItem(this Mock<IHttpContextAccessor> httpContextAccessor, KeyValuePair<object, object> pair)
        => httpContextAccessor.Object.HttpContext.Items.Should().Contain(pair);

}