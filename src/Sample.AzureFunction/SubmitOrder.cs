using System;

namespace Sample.AzureFunction
{
    // 定义 message interface，会由 MassTransit 来实现
    public interface SubmitOrder
    {
        Guid OrderId { get; }
        string OrderNumber { get; }
    }
}
