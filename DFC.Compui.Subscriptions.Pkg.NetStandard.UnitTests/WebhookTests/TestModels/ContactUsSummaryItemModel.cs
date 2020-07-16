using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels
{
    public class ContactUsSummaryItemModel
    {
        public Uri? Url { get; set; }

        public string? CanonicalName { get; set; }

        public DateTime Published { get; set; }
    }
}
