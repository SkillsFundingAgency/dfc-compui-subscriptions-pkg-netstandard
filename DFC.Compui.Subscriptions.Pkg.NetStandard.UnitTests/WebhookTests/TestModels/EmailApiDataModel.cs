using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels
{
    [ExcludeFromCodeCoverage]
    public class EmailApiDataModel
    {
        [JsonProperty("uri")]
        public Uri? Url { get; set; }

        public string? To { get; set; }

        public string? From { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
