using DFC.App.Subscription;
using DFC.Compui.Subscriptions.Pkg.Data;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Webhook.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubscriptionBackgroundService(
          this IServiceCollection services, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.Configure<SubscriptionSettings>(configuration.GetSection(nameof(SubscriptionSettings)) ?? throw new ArgumentException($"{nameof(SubscriptionSettings)} not present in AppSettings"));
            services.AddHostedService<SubscriptionRegistrationBackgroundService>();
            return services;
        }

        public static IServiceCollection AddSubscriptionService(
         this IServiceCollection services, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.Configure<SubscriptionSettings>(configuration.GetSection(nameof(SubscriptionSettings)) ?? throw new ArgumentException($"{nameof(SubscriptionSettings)} not present in AppSettings"));
            services.AddTransient<ISubscriptionRegistrationService, SubscriptionRegistrationService>();
            return services;
        }
    }
}
