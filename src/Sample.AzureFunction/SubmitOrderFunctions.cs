using System.Threading;
using System.Threading.Tasks;
using MassTransit.WebJobs.ServiceBusIntegration;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Sample.AzureFunction.Consumers;

namespace Sample.AzureFunction
{
    // POCO 实现 Azure Function
    public class SubmitOrderFunctions
    {
        private const string SubmitOrderQueueName = "submit-order";
        // 注入 MassTransit 的 message receiver，它和 Azure Service Bus 集成，由它来 dispatch 收到的消息
        readonly IMessageReceiver _receiver;

        public SubmitOrderFunctions(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }

        // 声明 Azure Function method
        [FunctionName("SubmitOrder")]
        public Task SubmitOrderAsync(
            // 指定 trigger 类型
            [ServiceBusTrigger(SubmitOrderQueueName)] Message message,
            CancellationToken cancellationToken)
        {
            // message receiver 将消息 dispatch 给指定消息的 consumer，这里是 SubmitOrderConsumer
            return _receiver.HandleConsumer<SubmitOrderConsumer>(SubmitOrderQueueName, message, cancellationToken);
        }
    }
}
