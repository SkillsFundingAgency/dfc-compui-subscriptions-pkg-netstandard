﻿using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Enums;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Services
{
    public class WebhookReceiver : IWebhookReceiver
    {
        private readonly ILogger<WebhookReceiver> logger;
        private readonly IWebhooksService webhookService;

        private readonly Dictionary<string, WebhookCacheOperation> acceptedEventTypes = new Dictionary<string, WebhookCacheOperation>
        {
            { "draft", WebhookCacheOperation.CreateOrUpdate },
            { "published", WebhookCacheOperation.CreateOrUpdate },
            { "draft-discarded", WebhookCacheOperation.Delete },
            { "unpublished", WebhookCacheOperation.Delete },
            { "deleted", WebhookCacheOperation.Delete },
        };

        public WebhookReceiver(ILogger<WebhookReceiver> logger, IWebhooksService webhookService)
        {
            this.logger = logger;
            this.webhookService = webhookService;
        }

        public async Task<IActionResult> ReceiveEvents(string eventContent)
        {
            var eventGridSubscriber = new EventGridSubscriber();
            foreach (var key in acceptedEventTypes.Keys)
            {
                eventGridSubscriber.AddOrUpdateCustomEventMapping(key, typeof(EventGridEventData));
            }

            var eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(eventContent);

            foreach (var eventGridEvent in eventGridEvents)
            {
                if (!Guid.TryParse(eventGridEvent.Id, out Guid eventId))
                {
                    throw new InvalidDataException($"Invalid Guid for EventGridEvent.Id '{eventGridEvent.Id}'");
                }

                if (eventGridEvent.Data is SubscriptionValidationEventData subscriptionValidationEventData)
                {
                    logger.LogInformation($"Got SubscriptionValidation event data, validationCode: {subscriptionValidationEventData!.ValidationCode},  validationUrl: {subscriptionValidationEventData.ValidationUrl}, topic: {eventGridEvent.Topic}");

                    // Do any additional validation (as required) such as validating that the Azure resource ID of the topic matches
                    // the expected topic and then return back the below response
                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = subscriptionValidationEventData.ValidationCode,
                    };

                    return new OkObjectResult(responseData);
                }
                else if (eventGridEvent.Data is EventGridEventData eventGridEventData)
                {
                    ValidateData(eventId, eventGridEventData, out Guid contentId, out Uri url);

                    var cacheOperation = acceptedEventTypes[eventGridEvent.EventType];

                    logger.LogInformation($"Got Event Id: {eventId}: {eventGridEvent.EventType}: Cache operation: {cacheOperation} {url}");

                    var result = await webhookService.ProcessMessageAsync(cacheOperation, eventId, contentId, url).ConfigureAwait(false);

                    LogResult(eventId, contentId, result);
                }
                else
                {
                    throw new InvalidDataException($"Invalid event type '{eventGridEvent.EventType}' received for Event Id: {eventId}, should be one of '{string.Join(",", acceptedEventTypes.Keys)}'");
                }
            }

            return new OkResult();
        }

        private static void ValidateData(Guid eventId, EventGridEventData eventGridEventData, out Guid contentId, out Uri url)
        {
            if (!Guid.TryParse(eventGridEventData.ItemId, out contentId))
            {
                throw new InvalidDataException($"Invalid Guid for EventGridEvent.Data.ItemId '{eventGridEventData.ItemId}'");
            }

            if (!Uri.TryCreate(eventGridEventData.Api, UriKind.Absolute, out var localUrl))
            {
                throw new InvalidDataException($"Invalid Api url '{eventGridEventData.Api}' received for Event Id: {eventId}");
            }

            url = localUrl ?? throw new InvalidDataException($"Invalid url '{localUrl}' received for Event Id: {eventId}");
        }

        private void LogResult(Guid eventId, Guid contentPageId, HttpStatusCode result)
        {
            switch (result)
            {
                case HttpStatusCode.OK:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Updated Content Page");
                    break;

                case HttpStatusCode.Created:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Created Content Page");
                    break;

                case HttpStatusCode.AlreadyReported:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Content Page previously updated");
                    break;

                default:
                    logger.LogWarning($"Event Id: {eventId}, Content Page Id: {contentPageId}: Content Page not Posted: Status: {result}");
                    break;
            }
        }
    }
}
