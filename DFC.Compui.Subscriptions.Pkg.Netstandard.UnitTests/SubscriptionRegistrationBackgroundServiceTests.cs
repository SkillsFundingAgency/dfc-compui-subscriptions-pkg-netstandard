using DFC.App.Subscription;
using DFC.Compui.Subscriptions.Pkg.Data;
using DFC.Compui.Subscriptions.Pkg.Netstandard.UnitTests.Utilities;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Subscriptions.Pkg.Netstandard.UnitTests
{
    public class SubscriptionRegistrationBackgroundServiceTests
    {
        private readonly IConfiguration configuration = A.Fake<IConfiguration>();
        private readonly IHttpClientFactory httpClientFactory = A.Fake<IHttpClientFactory>();
        private readonly ILogger<SubscriptionRegistrationBackgroundService> logger = A.Fake<ILogger<SubscriptionRegistrationBackgroundService>>();
        private readonly IOptionsMonitor<SubscriptionSettings> settings = A.Fake<IOptionsMonitor<SubscriptionSettings>>();

        public SubscriptionRegistrationBackgroundServiceTests()
        {
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoApplicationNameSettingThrowsException()
        {
            //Arrange
            var serviceToTest = new SubscriptionRegistrationBackgroundService(settings, configuration, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);

            serviceToTest.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceNoWebhookSettingThrowsException()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            var serviceToTest = new SubscriptionRegistrationBackgroundService(settings, configuration, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);

            serviceToTest.Dispose();
        }

        [Fact]
        public async Task SubscriptionRegistrationBackgroundServiceCorrectWebhookSettingReturnsSuccessful()
        {
            //Arrange
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("test-app");
            A.CallTo(() => settings.CurrentValue).Returns(new SubscriptionSettings { Endpoint = new Uri("https://somewebhookreceiver.com/receive"), SubscriptionServiceEndpoint = new Uri("https://somewheretosubscribeto.com") });

            var httpResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);
            A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

            var serviceToTest = new SubscriptionRegistrationBackgroundService(settings, configuration, httpClientFactory, logger);

            //Act
            await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            serviceToTest.Dispose();
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

            var serviceToTest = new SubscriptionRegistrationBackgroundService(settings, configuration, httpClientFactory, logger);

            //Act
            //Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await serviceToTest.StartAsync(CancellationToken.None).ConfigureAwait(false)).ConfigureAwait(false);
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();

            serviceToTest.Dispose();
            httpResponse.Dispose();
            fakeHttpMessageHandler.Dispose();
            httpClient.Dispose();
        }
    }
}