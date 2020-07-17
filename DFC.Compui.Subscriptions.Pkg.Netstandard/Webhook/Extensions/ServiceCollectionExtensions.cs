using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Webhook.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebhookSupport<TModel, TModelChild>(this IServiceCollection services)
            where TModel : class, IDocumentModel, IContentItemModel
            where TModelChild : class, IDocumentModel, IContentItemModel
        {
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<IWebhooksService, WebhooksService<TModel, TModelChild>>();

            return services;
        }

        public static IServiceCollection AddWebhookSupport<TModel>(this IServiceCollection services)
            where TModel : class, IDocumentModel, IContentItemModel
        {
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            _ = services.AddTransient<IWebhooksService, WebhooksService<TModel, NoChildren>>();

            return services;
        }
    }
}
