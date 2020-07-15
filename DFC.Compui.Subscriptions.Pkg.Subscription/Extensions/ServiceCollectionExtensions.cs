using DFC.App.Subscription;
using DFC.Compui.Subscriptions.Pkg.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventGridSubscription(
          this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SubscriptionSettings>(configuration.GetSection(nameof(SubscriptionSettings)) ?? throw new ArgumentException($"{nameof(SubscriptionSettings)} not present in AppSettings"));
            services.AddHostedService<SubscriptionRegistrationBackgroundService>();
            return services;
        }
    }
}
