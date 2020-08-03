using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Enums;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Models;
using Microsoft.Azure.Management.EventGrid.Models;
using System;
using System.Linq;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.Converters
{
    public static class EventGridFilterConverter
    {
        public static AdvancedFilter Convert(this ApiAdvancedFilter advancedFilter)
        {
            _ = advancedFilter ?? throw new ArgumentNullException(nameof(advancedFilter));

            return advancedFilter.Type switch
            {
                FilterTypeEnum.StringContains => new StringContainsAdvancedFilter(advancedFilter.Property, advancedFilter.Values),
                FilterTypeEnum.StringEndsWith => new StringEndsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values),
                FilterTypeEnum.StringIn => new StringInAdvancedFilter(advancedFilter.Property, advancedFilter.Values),
                FilterTypeEnum.StringBeginsWith => new StringBeginsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values),
                FilterTypeEnum.StringNotIn => new StringNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values),
                FilterTypeEnum.NumberNotIn => new NumberNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => double.Parse(x, null)).Cast<double?>().ToList()),
                FilterTypeEnum.NumberLessThanOrEquals => new NumberLessThanOrEqualsAdvancedFilter(advancedFilter.Property, double.Parse(advancedFilter.Values.FirstOrDefault(), null)),
                FilterTypeEnum.NumberLessThan => new NumberLessThanAdvancedFilter(advancedFilter.Property, double.Parse(advancedFilter.Values.FirstOrDefault(), null)),
                FilterTypeEnum.NumberIn => new NumberInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => double.Parse(x, null)).Cast<double?>().ToList()),
                FilterTypeEnum.NumberGreaterThanOrEquals => new NumberGreaterThanAdvancedFilter(advancedFilter.Property, double.Parse(advancedFilter.Values.FirstOrDefault(), null)),
                FilterTypeEnum.BoolEquals => new BoolEqualsAdvancedFilter(advancedFilter.Property, bool.Parse(advancedFilter.Values.FirstOrDefault())),
                _ => throw new NotSupportedException(nameof(advancedFilter.Type)),
            };
        }
    }
}
