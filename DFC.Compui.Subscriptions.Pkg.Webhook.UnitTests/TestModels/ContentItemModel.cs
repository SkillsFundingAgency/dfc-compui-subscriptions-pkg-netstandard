using DFC.Compui.Subscriptions.Pkg.Data.Models;
using System;
using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels
{
    public class ContentItemModel : IContentItemModel
    {
        public Uri? Url { get; set; }

        public Guid? ItemId { get; set; }

        public string? DisplayText { get; set; }

        public Guid Version { get; set; }

        public string? Content { get; set; }

        public int Justify { get; set; }

        public int Ordinal { get; set; }

        public int Width { get; set; }

        public DateTime? LastReviewed { get; set; }

        public IContentLinks ContentLinks { get; set; }

        public IList<IContentItemModel> ContentItems { get; set; }
    }
}
