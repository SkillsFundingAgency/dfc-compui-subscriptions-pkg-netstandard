using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Subscription
{
    public class SubscriptionRegistrationBackgroundService : BackgroundService
    {
        private readonly ISubscriptionRegistrationService subscriptionRegistrationService;

        public SubscriptionRegistrationBackgroundService(ISubscriptionRegistrationService subscriptionRegistrationService, ILogger<SubscriptionRegistrationBackgroundService> logger)
        {
            this.subscriptionRegistrationService = subscriptionRegistrationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.subscriptionRegistrationService.RegisterSubscription().ConfigureAwait(false);
        }
    }
}
