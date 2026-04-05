using Serilog;

namespace LTS.API.Infrastructure.Services.Extensions;

public static class SerilogExtension
{
    public static IHostBuilder AddSerilog(
        this IHostBuilder hostBuilder,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return hostBuilder.UseSerilog();
    }
}