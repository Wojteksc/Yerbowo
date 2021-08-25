using System.ComponentModel;

namespace Yerbowo.Domain.Orders
{
    public enum OrderStatus
    {
        [Description("Anulowane")]
        Canceled = 0,
        [Description("Nowe")]
        New = 1,
        [Description("Skompletowane")]
        Completed = 2,
        [Description("Wysłane")]
        Shipped = 3,
        [Description("Dostarczone")]
        Delivered = 4,
    }
}
