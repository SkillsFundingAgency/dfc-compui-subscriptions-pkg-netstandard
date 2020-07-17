using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class NoChildren : IContentItemModel, IDocumentModel
    {
        public Guid? ItemId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IContentLinks? ContentLinks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IList<IContentItemModel>? ContentItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Etag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
