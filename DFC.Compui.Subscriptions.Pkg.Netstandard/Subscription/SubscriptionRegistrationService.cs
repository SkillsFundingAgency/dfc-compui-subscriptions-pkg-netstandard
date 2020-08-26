using DFC.Compui.Subscriptions.Pkg.Data;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.Subscription.Services
{
    public class SubscriptionRegistrationService : ISubscriptionRegistrationService
    {
        private readonly IOptionsMonitor<SubscriptionSettings> settings;
        private readonly HttpClient httpClient;
        private readonly ILogger<SubscriptionRegistrationService> logger;

        public SubscriptionRegistrationService(IOptionsMonitor<SubscriptionSettings> settings, IHttpClientFactory httpClientFactory, ILogger<SubscriptionRegistrationService> logger)
        {
            this.settings = settings;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task RegisterSubscription(string subscriptionName)
        {
            logger.LogInformation("Subscription registration started");

            this.settings.CurrentValue.Filter?.IncludeEventTypes?.RemoveAll(r => string.IsNullOrWhiteSpace(r));
            this.ValidateSubscriptionSettings(this.settings.CurrentValue);

            var webhookReceiverUrl = $"{settings.CurrentValue.Endpoint ?? throw new ArgumentException(nameof(settings.CurrentValue.Endpoint))}";

            logger.LogInformation($"Registering subscription for endpoint: {webhookReceiverUrl}");

            var subscriptionRequest = settings.CurrentValue;
            subscriptionRequest.Name = subscriptionName;

            var content = new StringContent(JsonConvert.SerializeObject(subscriptionRequest), Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(this.settings.CurrentValue.ApiKey))
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.settings.CurrentValue.ApiKey);
            }

            var result = await httpClient.PostAsync(settings.CurrentValue.SubscriptionServiceEndpoint, content).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                logger.LogError(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
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
