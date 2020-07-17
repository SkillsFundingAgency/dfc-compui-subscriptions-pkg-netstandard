using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Data
{
    [ExcludeFromCodeCoverage]
    public class SubscriptionFilter
    {
        public string? BeginsWith { get; set; }

        public string? EndsWith { get; set; }

        public List<string>? IncludeEventTypes { get; set; }

        public List<SubscriptionPropertyContainsFilter>? PropertyContainsFilters { get; set; }
    }
}
