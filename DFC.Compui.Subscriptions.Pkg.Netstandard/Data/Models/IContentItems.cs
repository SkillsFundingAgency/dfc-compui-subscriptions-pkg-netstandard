using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public interface IContentItems
    {
        IList<IContentItemModel>? ContentItems { get; set; }
    }
}
