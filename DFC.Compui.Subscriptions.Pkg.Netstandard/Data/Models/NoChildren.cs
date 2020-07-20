using DFC.Compui.Cosmos.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class NoChildren : IContentItemModel, IDocumentModel
    {
        public Guid? ItemId { get; set; }

        public IContentLinks? ContentLinks { get; set; }

        public IList<IContentItemModel>? ContentItems { get; set; }

        public Guid Id { get; set; }

        public string? Etag { get; set; }

        public string? PartitionKey { get; set; }
    }
}
