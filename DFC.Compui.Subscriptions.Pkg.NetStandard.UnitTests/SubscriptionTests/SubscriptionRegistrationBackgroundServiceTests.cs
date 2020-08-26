using DFC.Compui.Subscriptions.Pkg.Data;
using DFC.Compui.Subscriptions.Pkg.Netstandard.UnitTests.Utilities;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Subscription.Services;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Subscriptions.Pkg.Netstandard.UnitTests
{
    public class SubscriptionRegistrationBackgroundServiceTests
    {
        private readonly IConfiguration configuration = A.Fake<IConfiguration>();
        private readonly IHttpClientFactory httpClientFactory = A.Fake<IHttpClientFactory>();
        private readonly ILogger<SubscriptionRegistrationService> logger = A.Fake<ILogger<SubscriptionRegistrationService>>();
        private readonly IOptionsMonitor<SubscriptionSettings> settings = A.Fake<IOptionsMonitor<SubscriptionSettings>>();

        public SubscriptionRegistrationBackgroundServiceTests()
        {
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoApplicationNameSettingThrowsException()
        {
            //Arrange
            var serviceToTest = new SubscriptionRegistrationService(settings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.RegisterSubscription("a-test-subscriber").ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoWebhookSettingThrowsException()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            var serviceToTest = new SubscriptionRegistrationService(settings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.RegisterSubscription("a-test-subscriber").ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsSuccessful()
        {
            //Arrange
            var subscriptionSettings = new SubscriptionSettings
            {
                Endpoint = new Uri("https://somewebhookreceiver.com/receive"),
                SubscriptionServiceEndpoint = new Uri("https://somewheretosubscribeto.com"),
                Filter = new SubscriptionFilter
                {
                    IncludeEventTypes = new List<string> { "published", "unpublished", "deleted" },
                },
            };
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            A.CallTo(() => settings.CurrentValue).Returns(subscriptionSettings);

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationService(settings, httpClientFactory, logger);

            //Act
            await serviceToTest.RegisterSubscription("a-test-subscriber").ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(3, subscriptionSettings.Filter.IncludeEventTypes.Count);

            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsSuccessfulWithBlankIncludeEventTypes()
        {
            //Arrange
            var subscriptionSettings = new SubscriptionSettings
            {
                Endpoint = new Uri("https://somewebhookreceiver.com/receive"),
                SubscriptionServiceEndpoint = new Uri("https://somewheretosubscribeto.com"),
                Filter= new SubscriptionFilter
                {
                    IncludeEventTypes = new List<string> { "published", "", "unpublished", "", null, "deleted" },
                },
            };
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            A.CallTo(() => settings.CurrentValue).Returns(subscriptionSettings);

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationService(settings, httpClientFactory, logger);

            //Act
            await serviceToTest.RegisterSubscription("a-test-subscriber").ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(3, subscriptionSettings.Filter.IncludeEventTypes.Count);

            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsDownstreamError()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            A.CallTo(() => settings.CurrentValue).Returns(new SubscriptionSettings { Endpoint = new Uri("https://somewebhookreceiver.com/receive"), SubscriptionServiceEndpoint = new Uri("https://somewheretosubscribeto.com") });

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError, Content = new StringContent("An error", Encoding.UTF8) };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationService(settings, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await serviceToTest.RegisterSubscription("a-test-subscriber").ConfigureAwait(false)).ConfigureAwait(false);
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }
    }
}
