namespace DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Models
{
    public class ApiAdvancedFilter
    {
        public string? Property { get; set; }

        public FilterTypeEnum Type { get; set; }

        public List<string>? Values { get; set; }
    }
}
