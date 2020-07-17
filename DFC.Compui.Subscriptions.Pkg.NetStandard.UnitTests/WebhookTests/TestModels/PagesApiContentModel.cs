﻿using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.UnitTests.WebhookTests.TestModels
{
    public class PagesApiContentItemModel : IApiDataModel, IContentItemModel
    {
        public Uri? Url { get; set; }

        [JsonProperty("id")]
        public Guid? ItemId { get; set; }

        public string? DisplayText { get; set; }

        public Guid Version { get; set; }

        public string? Content { get; set; }

        public int? Justify { get; set; }

        public string? Alignment { get; set; }

        public int? Ordinal { get; set; }

        public int? Width { get; set; }

        public int? Size { get; set; }

        [JsonProperty(PropertyName = "ModifiedDate")]
        public DateTime Published { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? Href { get; set; }

        public string? Title { get; set; }

        public string? ContentType { get; set; }

        [JsonProperty("htmlbody_Html")]
        public string? HtmlBody { get; set; }

        [JsonProperty("_links")]
        public JObject? Links { get; set; }

        [JsonIgnore]
        public IContentLinks? ContentLinks
        {
            get => PrivateLinksModel ??= new ContentLinksModel(Links ?? new JObject());

            set => PrivateLinksModel = value;
        }

        [JsonIgnore]
        public IList<PagesApiContentItemModel> ContentItems { get; set; } = new List<PagesApiContentItemModel>();

        [JsonIgnore]
        private IContentLinks? PrivateLinksModel { get; set; }

        IList<IContentItemModel>? IContentItemModel.ContentItems { get; set; }
    }
}