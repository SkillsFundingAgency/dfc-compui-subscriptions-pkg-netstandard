using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public interface IContentLinks
    {
        List<KeyValuePair<string, List<LinkDetails>>> ContentLinks { get; set; }
    }
}
