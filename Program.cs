using Integradora.Consumer.RabbitMQ;
using Integradora.Publish.RabbitMQ.Extentions;

var builder = Host.CreateDefaultBuilder(args);

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
ConfigureAppSettings(configurationBuilder);
IConfiguration configuration = configurationBuilder.Build();

builder.ConfigureServices((services) =>
{
    services.AddDependency();
    services.AddHostedService<Worker>();
    services.ConfigurationAppSettings(configuration);
});

var host = builder.Build();

host.Run();

static void ConfigureAppSettings(IConfigurationBuilder configuration)
{
    var tipoDepuracao = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var diretorio = Path.Combine(Directory.GetCurrentDirectory(), !string.IsNullOrEmpty(tipoDepuracao) ? $"appsettings.{tipoDepuracao}.json" : "appsettings.json");

    configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(diretorio, optional: true, reloadOnChange: true);
}