using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities.Commands;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;
using WumpWump.Net.Gateway.Modules;
using WumpWump.Net.Rest;

namespace WumpWump.Net.Gateway
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordGatewayClient(this IServiceCollection services, string discordToken)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentException.ThrowIfNullOrWhiteSpace(discordToken, nameof(discordToken));

            services.AddDiscordRestClient(discordToken);
            services.TryAddKeyedSingleton("WumpWump.Net.Gateway", (serviceProvider, key) =>
            {
                DiscordGatewayEventRegistration eventRegistrations = serviceProvider.GetRequiredService<DiscordGatewayEventRegistration>();
                JsonSerializerOptions jsonSerializerOptions = serviceProvider.GetRequiredKeyedService<JsonSerializerOptions>("WumpWump.Net.Rest");
                jsonSerializerOptions.Converters.Add(new DiscordGatewayPayloadJsonConverter(eventRegistrations));
                return jsonSerializerOptions;
            });

            services.TryAddKeyedSingleton<HttpMessageInvoker>("WumpWump.Net.Gateway", (serviceProvider, key) =>
            {
                HttpClient client = new()
                {
                    DefaultRequestVersion = HttpVersion.Version11,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                    Timeout = serviceProvider.GetRequiredService<DiscordRestClientOptions>().Timeout
                };

                client.DefaultRequestHeaders.UserAgent.ParseAdd(DiscordGatewayIdentifyCommandProperties.DISCORD_USER_AGENT);
                return client;
            });

            services.TryAddSingleton((serviceProvider) =>
            {
                AsyncEventContainer asyncEventContainer = new();
                foreach (Type type in typeof(DiscordGatewayClient).Assembly.GetTypes())
                {
                    foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        if (methodInfo.GetCustomAttribute<AsyncEventHandlerAttribute>() is not null)
                        {
                            asyncEventContainer.AddHandlers(type, ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, type));
                            break;
                        }
                    }
                }

                return asyncEventContainer;
            });

            services.TryAddSingleton((serviceProvider) =>
            {
                DiscordGatewayEventRegistration eventRegistrations = [];
                foreach (Type type in typeof(DiscordGatewayClient).Assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<DiscordGatewayEventAttribute>() is not DiscordGatewayEventAttribute gatewayEventAttribute)
                    {
                        continue;
                    }

                    Type gatewayRegistrationGenericType = typeof(DiscordGatewayEventTicket<>).MakeGenericType(type);
                    object gatewayRegistrationObj = Activator.CreateInstance(gatewayRegistrationGenericType, [gatewayEventAttribute.OpCode, gatewayEventAttribute.EventName])
                        ?? throw new InvalidOperationException($"Failed to create instance of {gatewayRegistrationGenericType}");

                    MethodInfo addHandlerMethod = typeof(ServiceCollectionExtensions).GetMethod(nameof(AddHandler), BindingFlags.NonPublic | BindingFlags.Static)
                        ?? throw new InvalidOperationException($"Failed to find method {nameof(AddHandler)}");

                    MethodInfo genericAddHandlerMethod = addHandlerMethod.MakeGenericMethod(type);
                    genericAddHandlerMethod.Invoke(null, [gatewayRegistrationObj, eventRegistrations]);
                }

                return eventRegistrations;
            });

            services.TryAddTransient<IDiscordGatewayMessageModule, DiscordGatewayMessageModule>();
            services.TryAddSingleton<IDiscordGatewayEventModule, DiscordGatewayEventModule>();
            services.TryAddSingleton<DiscordGatewayClient>();
            return services;
        }

        private static void AddHandler<T>(object gatewayRegistrationObj, DiscordGatewayEventRegistration eventRegistrations)
            => eventRegistrations.Add((DiscordGatewayEventTicket<T>)gatewayRegistrationObj);
    }
}
