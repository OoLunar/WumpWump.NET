using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Events;
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
            services.AddSingleton<DiscordGatewayClient>();
            services.AddSingleton((serviceProvider) =>
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

            services.AddSingleton((serviceProvider) =>
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

            return services;
        }

        private static void AddHandler<T>(object gatewayRegistrationObj, DiscordGatewayEventRegistration eventRegistrations)
            => eventRegistrations.Add((DiscordGatewayEventTicket<T>)gatewayRegistrationObj);
    }
}
