using DFC.Compui.Cosmos;
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
        /// <summary>
        /// Add webhook support with a custom implementation for IWebhookService.
        /// </summary>
        /// <typeparam name="TWebhookService">The custom webhook service.</typeparam>
        /// <param name="services">The services collection.</param>
        /// <returns>The <see cref="IServiceCollection"/>. </returns>
        public static IServiceCollection AddWebhookSupport<TWebhookService>(this IServiceCollection services)
            where TWebhookService : class, IWebhooksService
        {
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<IWebhooksService, TWebhookService>();

            return services;
        }

        /// <summary>
        /// Add webhook support with the default implementation of IWebhookService for a model with children.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The <see cref="IServiceCollection"/>. </returns>
        public static IServiceCollection AddWebhookSupportWithChildren<TModel, TModelChild>(this IServiceCollection services)
            where TModel : class, IDocumentModel, IContentItemModel
            where TModelChild : class, IDocumentModel, IContentItemModel
        {
            AddBaseService<TModel>(services);
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<IWebhooksService, WebhooksService<TModel, TModelChild>>();

            return services;
        }

        /// <summary>
        /// Add webhook support with the default implementation of IWebhookService for a model without children.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The <see cref="IServiceCollection"/>. </returns>
        public static IServiceCollection AddWebhookSupportNoChildren<TModel>(this IServiceCollection services)
            where TModel : class, IDocumentModel, IContentItemModel
        {
            AddBaseService<TModel>(services);
            services.AddTransient<IWebhookReceiver, WebhookReceiver>();
            services.AddTransient<IWebhooksService, WebhooksService<TModel, NoChildren>>();

            return services;
        }

        private static IServiceCollection AddBaseService<TModel>(this IServiceCollection services)
             where TModel : class, IDocumentModel, IContentItemModel
        {
            services.AddTransient<IEventMessageService<TModel>, EventMessageService<TModel>>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<IApiDataProcessorService, ApiDataProcessorService>();
            services.AddTransient<IEventMessageService<TModel>, EventMessageService<TModel>>();

            return services;
        }
    }
}
