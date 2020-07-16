﻿using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels
{
    public class EmailModel : IDocumentModel, IContentItemModel
    {
        public EmailModel()
        {
            PartitionKey = "Email";
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        public string? Body { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        public string? PartitionKey { get; set; }

        public Guid? ItemId { get; set; }

        public IContentLinks? ContentLinks { get; set; }

        public IList<IContentItemModel>? ContentItems { get; set; }
    }
}
