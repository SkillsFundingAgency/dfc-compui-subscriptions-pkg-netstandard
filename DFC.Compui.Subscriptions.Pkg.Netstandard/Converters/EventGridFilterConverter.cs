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
                FilterType.StringContains => new StringContainsAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterType.StringEndsWith => new StringEndsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterType.StringIn => new StringInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterType.StringBeginsWith => new StringBeginsWithAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterType.StringNotIn => new StringNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (string)x).ToList()),
                FilterType.NumberNotIn => new NumberNotInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).ToList()),
                FilterType.NumberLessThanOrEquals => new NumberLessThanOrEqualsAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberLessThanOrEqualsAdvancedFilter)}")),
                FilterType.NumberLessThan => new NumberLessThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberLessThanAdvancedFilter)}")),
                FilterType.NumberIn => new NumberInAdvancedFilter(advancedFilter.Property, advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).ToList()),
                FilterType.NumberGreaterThanOrEquals => new NumberGreaterThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberGreaterThanOrEqualsAdvancedFilter)}")),
                FilterType.NumberGreaterThan => new NumberGreaterThanAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => (double?)double.Parse(x.ToString(), null)).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(NumberGreaterThanAdvancedFilter)}")),
                FilterType.BoolEquals => new BoolEqualsAdvancedFilter(advancedFilter.Property, advancedFilter.Values?.Count == 1 ? advancedFilter.Values.Select(x => bool.Parse(x.ToString())).FirstOrDefault() : throw new InvalidOperationException($"Only one filter can be passed to {nameof(BoolEqualsAdvancedFilter)}")),
                _ => throw new NotSupportedException(nameof(advancedFilter.Type)),
            };
        }
    }
}
