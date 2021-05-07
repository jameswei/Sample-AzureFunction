using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Context;

namespace Sample.AzureFunction.Consumers
{
    // 通过 IConsumer 接口来实现指定消息类型的 consumer
    public class SubmitOrderConsumer :
        IConsumer<SubmitOrder>
    {
        // 由 MassTransit 将收到的消息封装为 ConsumerContext
        public Task Consume(ConsumeContext<SubmitOrder> context)
        {
            LogContext.Debug?.Log("Processing Order: {OrderNumber}", context.Message.OrderNumber);

            context.Publish<OrderReceived>(new
            {
                context.Message.OrderId,
                context.Message.OrderNumber,
                Timestamp = DateTime.UtcNow
            });

            return context.RespondAsync<OrderAccepted>(new { context.Message.OrderId, context.Message.OrderNumber });
        }
    }
}
