using DFC.Compui.Subscriptions.Pkg.Data.Enums;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests
{
    [Trait("Category", "Webhooks Controller Unit Tests")]
    public class WebhookReceiverTests : BaseWebhookReceiverTests
    {
        public static IEnumerable<object[]> PublishedEvents => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json, EventTypePublished },
            new object[] { MediaTypeNames.Application.Json, EventTypeDraft },
        };

        public static IEnumerable<object[]> DeletedEvents => new List<object[]>
        {
            new object[] { MediaTypeNames.Application.Json, EventTypeDraftDiscarded },
            new object[] { MediaTypeNames.Application.Json, EventTypeDeleted },
            new object[] { MediaTypeNames.Application.Json, EventTypeUnpublished },
        };

        public static IEnumerable<object[]> InvalidIdValues => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { "Not a Guid" },
        };

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishCreatePostReturnsOkForCreate(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { ItemId = ItemIdForCreate.ToString(), Api = "https://somewhere.com", });
            var service = BuildWebhookService(mediaTypeName);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishUpdatePostReturnsOk(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { ItemId = ItemIdForUpdate.ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(mediaTypeName);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await controller.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(DeletedEvents))]
        public async Task WebhooksControllerDeletePostReturnsSuccess(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { ItemId = ItemIdForDelete.ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(mediaTypeName);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await controller.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishCreatePostReturnsOkForAlreadyReported(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { ItemId = ItemIdForCreate.ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(mediaTypeName);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).Returns(HttpStatusCode.AlreadyReported);

            // Act
            var result = await controller.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(PublishedEvents))]
        public async Task WebhooksControllerPublishCreatePostReturnsOkForConflict(string mediaTypeName, string eventType)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var eventGridEvents = BuildValidEventGridEvent(eventType, new EventGridEventData { ItemId = ItemIdForCreate.ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(mediaTypeName);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).Returns(HttpStatusCode.Conflict);

            // Act
            var result = await controller.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<OkResult>(result);

            Assert.Equal((int)expectedResponse, okResult.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidIdValues))]
        public async Task WebhooksControllerPostReturnsErrorForInvalidEventId(string id)
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent(EventTypePublished, new EventGridEventData { ItemId = Guid.NewGuid().ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(MediaTypeNames.Application.Json);
            eventGridEvents.First().Id = id;
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveEvents(eventContent).ConfigureAwait(false)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
        }

        [Theory]
        [MemberData(nameof(InvalidIdValues))]
        public async Task WebhooksControllerPostReturnsErrorForInvalidItemId(string id)
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent(EventTypePublished, new EventGridEventData { ItemId = id, Api = "https://somewhere.com", });
            var controller = BuildWebhookService(MediaTypeNames.Application.Json);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveEvents(eventContent).ConfigureAwait(false)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task WebhooksControllerPostReturnsErrorForUnknownEventType()
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent("Unknown", new EventGridEventData { ItemId = Guid.NewGuid().ToString(), Api = "https://somewhere.com", });
            var controller = BuildWebhookService(MediaTypeNames.Application.Json); 
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveEvents(eventContent).ConfigureAwait(false)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task WebhooksControllerPostReturnsErrorForInvalidUrl()
        {
            // Arrange
            var eventGridEvents = BuildValidEventGridEvent(EventTypePublished, new EventGridEventData { Api = "http:http://badUrl" });
            var controller = BuildWebhookService(MediaTypeNames.Application.Json); 
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await controller.ReceiveEvents(eventContent).ConfigureAwait(false)).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task WebhooksControllerSubscriptionValidationReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            string expectedValidationCode = Guid.NewGuid().ToString();
            var eventGridEvents = BuildValidEventGridEvent(Microsoft.Azure.EventGrid.EventTypes.EventGridSubscriptionValidationEvent, new SubscriptionValidationEventData(expectedValidationCode, "https://somewhere.com"));
            var controller = BuildWebhookService(MediaTypeNames.Application.Json);
            var eventContent = await BuildStringContentFromModel(eventGridEvents);

            // Act
            var result = await controller.ReceiveEvents(eventContent).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeWebhooksService.ProcessMessageAsync(A<WebhookCacheOperation>.Ignored, A<Guid>.Ignored, A<Guid>.Ignored, A<Uri>.Ignored)).MustNotHaveHappened();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<SubscriptionValidationResponse>(jsonResult.Value);

            Assert.Equal((int)expectedResponse, jsonResult.StatusCode);
            Assert.Equal(expectedValidationCode, response.ValidationResponse);
        }
    }
}
