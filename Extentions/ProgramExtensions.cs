using Integradora.Consumer.RabbitMQ.ConsumerService;
using Integradora.Consumer.RabbitMQ.Interfaces;
using Integradora.Consumer.RabbitMQ.Models.AppSettings;

namespace Integradora.Publish.RabbitMQ.Extentions
{
    public static class ProgramExtensions
    {
        public static void ConfigurationAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMqConfiguration"));
        }

        public static void AddDependency(this IServiceCollection services)
        {
            services.AddSingleton<IConsumerMessage, ConsumerMessage>();
        }
    }
}
