using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using WumpWump.Net.Gateway;

namespace WumpWump.Net.Examples.HelloWorld
{
    public sealed class Program
    {
        public static async Task<int> Main()
        {
            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (discordToken is null)
            {
                Console.WriteLine("Please set the DISCORD_TOKEN environment variable.");
                return 1;
            }

            ServiceCollection services = [];
            services.AddLogging(logging =>
            {
                IServiceProvider serviceProvider = logging.Services.BuildServiceProvider();
                LoggerConfiguration serilogLoggerConfiguration = new();
                serilogLoggerConfiguration.MinimumLevel.Is(LogEventLevel.Verbose);
                serilogLoggerConfiguration.WriteTo.Console(
                    formatProvider: CultureInfo.InvariantCulture,
                    outputTemplate: "[{Timestamp:O}] [{Level:u4}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code
                );

                logging.AddSerilog(serilogLoggerConfiguration.CreateLogger());
            });

            services.AddDiscordGatewayClient(discordToken);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            DiscordGatewayClient discordGatewayClient = serviceProvider.GetRequiredService<DiscordGatewayClient>();
            await discordGatewayClient.ConnectAsync();
            await Task.Delay(-1);
            return 0;
        }
    }
}
