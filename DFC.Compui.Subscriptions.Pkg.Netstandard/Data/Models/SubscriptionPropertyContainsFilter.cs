using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Compui.Subscriptions.Pkg.Data
{
    public class SubscriptionPropertyContainsFilter
    {
        public string? Key { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
        public string[]? Values { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
