﻿using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using Newtonsoft.Json;
using System;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.UnitTests.WebhookTests.TestModels
{
    public class PagesSummaryItemModel : IApiDataModel
    {
        [JsonProperty(PropertyName = "uri")]
        public Uri? Url { get; set; }

        [JsonProperty(PropertyName = "skos__prefLabel")]
        public string? Title { get; set; }

        [JsonProperty(PropertyName = "alias_alias")]
        public string? CanonicalName { get; set; }

        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "ModifiedDate")]
        public DateTime Published { get; set; }
    }
}