using DFC.Compui.Subscriptions.Pkg.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Subscription
{
    public class SubscriptionRegistrationBackgroundService : BackgroundService
    {
        private readonly IOptionsMonitor<SubscriptionSettings> settings;
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;
        private readonly ILogger<SubscriptionRegistrationBackgroundService> logger;

        public SubscriptionRegistrationBackgroundService(IOptionsMonitor<SubscriptionSettings> settings, IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<SubscriptionRegistrationBackgroundService> logger)
        {
            this.settings = settings;
            this.configuration = configuration;
            this.httpClient = httpClientFactory.CreateClient();
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.RegisterSubscription().ConfigureAwait(false);
        }

        private async Task RegisterSubscription()
        {
            logger.LogInformation("Subscription registration started");

            this.ValidateSubscriptionSettings(this.settings.CurrentValue);

            var subscribeName = !string.IsNullOrEmpty(configuration["Configuration:ApplicationName"]) ? configuration["Configuration:ApplicationName"] : throw new ArgumentException("Configuration:ApplicationName not present in IConfiguration");

            var webhookReceiverUrl = $"{settings.CurrentValue.Endpoint ?? throw new ArgumentException(nameof(settings.CurrentValue.Endpoint))}";

            logger.LogInformation($"Registering subscription for endpoint: {webhookReceiverUrl}");

            var subscriptionRequest = settings.CurrentValue;
            subscriptionRequest.Name = subscribeName;

            var content = new StringContent(JsonConvert.SerializeObject(subscriptionRequest), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(settings.CurrentValue.SubscriptionServiceEndpoint, content).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError(await result.Content.ReadAsStringAsync());
                throw new HttpRequestException($"Add subscription returned unsuccessful status code: {result.StatusCode}");
            }

            content.Dispose();

            logger.LogInformation("Subscription registration completed");
        }

        private void ValidateSubscriptionSettings(SubscriptionSettings settings)
        {
            logger.LogInformation("Validating subscription settings");

            if (settings.Endpoint == null)
            {
                throw new ArgumentException(nameof(settings.Endpoint));
            }

            if (settings.SubscriptionServiceEndpoint == null)
            {
                throw new ArgumentException(nameof(settings.SubscriptionServiceEndpoint));
            }

            logger.LogInformation("Validating subscription complete");
        }
    }
}
