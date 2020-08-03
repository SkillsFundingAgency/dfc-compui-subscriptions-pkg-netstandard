using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Enums;
using System.Collections.Generic;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Models
{
    public class ApiAdvancedFilter
    {
        public string? Property { get; set; }

        public FilterTypeEnum Type { get; set; }

        public List<object>? Values { get; set; }
    }
}
