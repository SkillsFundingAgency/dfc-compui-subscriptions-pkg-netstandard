using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Enums;
using DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels;
using FakeItEasy;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.WebhookServiceTests
{
    [Trait("Category", "Webhooks Service ProcessMessageAsync Unit Tests")]
    public class WebhooksServiceProcessMessageTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncNoneOptionReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var url = new Uri($"https://somewhere.com/email/{Guid.NewGuid()}");
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), Guid.NewGuid(), url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeMapper.Map<EmailModel>(A<EmailApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailEventMessageService.UpdateAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailEventMessageService.CreateAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeEmailEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateReturnsSuccess()
        {
            // Arrange

            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailModel, NoChildren>(A<Uri>.Ignored)).Returns(new EmailModel() { Body = "ATestBody", Id = Guid.NewGuid() });
            A.CallTo(() => FakeEmailEventMessageService.UpdateAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.Created);

            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var url = new Uri($"https://somewhere.com/email/{Guid.NewGuid()}");
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), Guid.NewGuid(), url).ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateReturnsOk()
        {
            // Arrange
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailModel, NoChildren>(A<Uri>.Ignored)).Returns(new EmailModel() { Body = "ATestBody", Id = Guid.NewGuid() });
            A.CallTo(() => FakeEmailEventMessageService.UpdateAsync(A<EmailModel>.Ignored)).Returns(HttpStatusCode.OK);

            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = new Uri($"https://somewhere.com/email/{Guid.NewGuid()}");
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), Guid.NewGuid(), url).ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteSharedContentReturnsNotFound()
        {
            // Arrange
            A.CallTo(() => FakeCmsApiService.GetItemAsync<EmailModel, NoChildren>(A<Uri>.Ignored)).Returns(new EmailModel() { Body = "ATestBody", Id = Guid.NewGuid() });
            A.CallTo(() => FakeEmailEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NotFound);

            const HttpStatusCode expectedResponse = HttpStatusCode.NotFound;
            var url = new Uri($"https://somewhere.com/sharedcontent/{Guid.NewGuid()}");
            var service = BuildWebhooksService();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), Guid.NewGuid(), url).ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteSharedContentReturnsok()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = new Uri($"https://somewhere.com/email/{Guid.NewGuid()}");
            var service = BuildWebhooksService();

            A.CallTo(() => FakeEmailEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), Guid.NewGuid(), url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEmailEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened();

            Assert.Equal(expectedResponse, result);
        }
    }
}
