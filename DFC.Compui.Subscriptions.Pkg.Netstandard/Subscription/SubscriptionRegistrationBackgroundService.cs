using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Subscription
{
    public class SubscriptionRegistrationBackgroundService : BackgroundService
    {
        private readonly ISubscriptionRegistrationService subscriptionRegistrationService;
        private readonly ILogger<SubscriptionRegistrationBackgroundService> logger;
        private readonly IConfiguration configuration;

        public SubscriptionRegistrationBackgroundService(ISubscriptionRegistrationService subscriptionRegistrationService, ILogger<SubscriptionRegistrationBackgroundService> logger, IConfiguration configuration)
        {
            this.subscriptionRegistrationService = subscriptionRegistrationService;
            this.logger = logger;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(SubscriptionRegistrationBackgroundService)} - {nameof(ExecuteAsync)} called");
            await this.subscriptionRegistrationService.RegisterSubscription(configuration["Configuration:ApplicationName"]).ConfigureAwait(false);
        }
    }
}
