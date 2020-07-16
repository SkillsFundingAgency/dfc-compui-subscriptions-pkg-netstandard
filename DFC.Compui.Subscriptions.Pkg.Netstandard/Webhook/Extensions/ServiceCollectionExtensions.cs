using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Webhook.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebhookSupport<TModel>(this IServiceCollection services)
            where TModel : class, IDocumentModel, IContentItemModel
        {
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<IWebhooksService, WebhooksService<TModel>>();

            return services;
        }
    }
}
