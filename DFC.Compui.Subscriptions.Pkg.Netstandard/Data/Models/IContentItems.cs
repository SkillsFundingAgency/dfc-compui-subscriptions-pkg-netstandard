using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public interface IContentItems
    {
        IList<IContentItemModel>? ContentItems { get; set; }
    }
}
