using System;

namespace DFC.Compui.Subscriptions.Pkg.Data
{
    public class SubscriptionSettings
    {
        public string? Name { get; set; }

        public Uri? SubscriptionServiceEndpoint { get; set; }

        public Uri? Endpoint { get; set; }

        public SubscriptionFilter? Filter { get; set; }
    }
}
