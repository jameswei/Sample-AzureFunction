using System.Threading;
using System.Threading.Tasks;
using MassTransit.WebJobs.EventHubsIntegration;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Sample.AzureFunction.Consumers;

namespace Sample.AzureFunction
{
    // 另一个 Azure Function 实现
    public class AuditOrderFunctions
    {
        private const string AuditOrderEventHubName = "audit-order";
        // 注入 event receiver，是 MassTransit 用来和 Azure Event Hub 集成的 receiver
        // 类似于 IMessageReceiver，dispatch 消息给具体的 consumer 实现
        readonly IEventReceiver _receiver;

        public AuditOrderFunctions(IEventReceiver receiver)
        {
            _receiver = receiver;
        }

        // 声明 Azure Function method
        [FunctionName("AuditOrder")]
        public Task AuditOrderAsync(
            // 所以 trigger 类型指定为 EventHubTrigger，而非 ServiceBusTrigger
            [EventHubTrigger(AuditOrderEventHubName, Connection = "AzureWebJobsEventHub")] EventData message,
            CancellationToken cancellationToken)
        {
            return _receiver.HandleConsumer<AuditOrderConsumer>(AuditOrderEventHubName, message, cancellationToken);
        }
    }
}
