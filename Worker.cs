using Integradora.Consumer.RabbitMQ.Interfaces;

namespace Integradora.Consumer.RabbitMQ
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerMessage _consumerMessage;

        public Worker(ILogger<Worker> logger, IConsumerMessage consumerMessage)
        {
            _logger = logger;
            _consumerMessage = consumerMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("O SERVI�O EST� INICIANDO.");

            stoppingToken.Register(() => _logger.LogInformation("Tarefa de segundo plano est� parando."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("[*] Iniciando Busca de Mensagens... : {time}", DateTimeOffset.Now);

                _consumerMessage.ConsumerMessageRabbitMQ();

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}