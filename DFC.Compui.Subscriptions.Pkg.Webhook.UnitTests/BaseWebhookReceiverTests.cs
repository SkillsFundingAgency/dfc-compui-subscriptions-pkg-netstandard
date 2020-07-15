using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Webhook.Controllers;
using DFC.Compui.Subscriptions.Pkg.Webhook.Services;
using DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests
{
    public abstract class BaseWebhookReceiverTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected const string ContentTypeContactUs = "contact-us";

        protected BaseWebhookReceiverTests()
        {
            Logger = A.Fake<ILogger<WebhookReceiver>>();
            FakeWebhooksService = A.Fake<IWebhooksService>();
        }

        protected Guid ItemIdForCreate { get; } = Guid.NewGuid();

        protected Guid ItemIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ItemIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhookReceiver> Logger { get; }

        protected IWebhooksService FakeWebhooksService { get; }

        protected static EventGridEvent[] BuildValidEventGridEvent<TModel>(string eventType, TModel data)
        {
            var models = new EventGridEvent[]
            {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"{ContentTypeContactUs}/a-canonical-name",
                    Data = data,
                    EventType = eventType,
                    EventTime = DateTime.Now,
                    DataVersion = "1.0",
                },
            };

            return models;
        }

        protected async static Task<string> BuildStringContentFromModel<TModel>(TModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            MemoryStream stream = new MemoryStream(byteArray);

            using var reader = new StreamReader(stream, Encoding.UTF8);
            string requestContent = await reader.ReadToEndAsync().ConfigureAwait(false);

            return requestContent;
        }

        protected WebhookReceiver BuildWebhookService(string mediaTypeName)
        {
            var objectValidator = A.Fake<IObjectModelValidator>();
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var receiver = new WebhookReceiver(Logger, FakeWebhooksService);

            return receiver;
        }
    }
}
