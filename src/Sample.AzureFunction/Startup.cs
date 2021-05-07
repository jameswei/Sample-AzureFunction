using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Sample.AzureFunction;
using Sample.AzureFunction.Consumers;

[assembly: FunctionsStartup(typeof(Startup))]


namespace Sample.AzureFunction
{
    // 继承 ASB 的 startup
    public class Startup :
        FunctionsStartup
    {
        // 重写 Configure() 来配置 MassTransit
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
            // 向 DI container 注册 scoped service，这里是两个 ASB function
                .AddScoped<SubmitOrderFunctions>()
                .AddScoped<AuditOrderFunctions>()
                // 配置 MassTransit，使用 ASB 作为 transport provider
                .AddMassTransitForAzureFunctions(cfg =>
                {
                    // 类似于 scan package，注册 assembly 中同一个 namespace 以及子 namespace 中所有的 IConsumer 实现类
                    // 这里通过类型参数指定了 anchor type 是 ConsumerNamespace
                    cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                })
                .AddMassTransitEventHub();
        }
    }
}