﻿using DFC.Compui.Subscriptions.Pkg.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IWebhooksService
    {
        Task<HttpStatusCode> DeleteContentAsync(Guid contentId);

        Task<HttpStatusCode> DeleteContentItemAsync(Guid contentItemId);

        Task<HttpStatusCode> ProcessContentAsync(Uri url, Guid contentId);

        Task<HttpStatusCode> ProcessContentItemAsync(Uri url, Guid contentItemId);

        Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, Uri url);
    }
}