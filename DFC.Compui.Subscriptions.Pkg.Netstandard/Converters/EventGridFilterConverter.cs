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
                FilterTypeEnum.StringContains => new StringContainsAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterTypeEnum.StringEndsWith => new StringEndsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterTypeEnum.StringIn => new StringInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterTypeEnum.StringBeginsWith => new StringBeginsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterTypeEnum.StringNotIn => new StringNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterTypeEnum.NumberNotIn => new NumberNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).ToList()),
                FilterTypeEnum.NumberLessThanOrEquals => new NumberLessThanOrEqualsAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberLessThanOrEqualsAdvancedFilter)}")),
                FilterTypeEnum.NumberLessThan => new NumberLessThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberLessThanAdvancedFilter)}")),
                FilterTypeEnum.NumberIn => new NumberInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).ToList()),
                FilterTypeEnum.NumberGreaterThanOrEquals => new NumberGreaterThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberGreaterThanOrEqualsAdvancedFilter)}")),
                FilterTypeEnum.NumberGreaterThan => new NumberGreaterThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberGreaterThanAdvancedFilter)}")),
                FilterTypeEnum.BoolEquals => new BoolEqualsAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => bool.Parse(x.ToString())).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(BoolEqualsAdvancedFilter)}")),
                _ => throw new NotSupportedException(nameof(advancedFilter.Type)),
            };
        }
    }
}
