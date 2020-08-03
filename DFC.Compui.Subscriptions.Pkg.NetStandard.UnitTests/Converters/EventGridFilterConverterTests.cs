using DFC.Compui.Subscriptions.Pkg.NetStandard.Converters;
using DFC.Compui.Subscriptions.Pkg.NetStandard.Data.Models;
using Microsoft.Azure.Management.EventGrid.Models;
using System.Collections.Generic;
using Xunit;

namespace DFC.Compui.Subscriptions.Pkg.NetStandard.UnitTests.Converters
{
    public class EventGridFilterConverterTests
    {
        [Theory]
        [MemberData(nameof(GetAdvancedFilters))]
        public void DoSomething(ApiAdvancedFilter filter, string destinationFilterName)
        {
            // Arrange
            // Act
            var result = filter.Convert();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Key);
            Assert.Equal(destinationFilterName, result.GetType().Name);
        }

        public static IEnumerable<object[]> GetAdvancedFilters()
        {
            var allData = new List<object[]>
        {
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.BoolEquals, Property = "Subject", Values = new List<object> { "true" }  }, nameof(BoolEqualsAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberGreaterThanOrEquals, Property = "Subject", Values = new List<object> { 2 } }, nameof(NumberGreaterThanAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberIn, Property = "Subject", Values = new List<object> { 3,4,5 } }, nameof(NumberInAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberLessThan, Property = "Subject", Values = new List<object> { 5 } }, nameof(NumberLessThanAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberLessThanOrEquals, Property = "Subject", Values = new List<object> { 6 } },  nameof(NumberLessThanOrEqualsAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberNotIn, Property = "Subject", Values = new List<object>{ 7,8,9 }  }, nameof(NumberNotInAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.StringBeginsWith, Property = "Subject", Values = new List<object> { "test" } }, nameof(StringBeginsWithAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.StringContains, Property = "Subject", Values = new List<object> { "a", "test", "word" } }, nameof(StringContainsAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.StringEndsWith, Property = "Subject", Values = new List<object> { "a", "test", "end" } }, nameof(StringEndsWithAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.StringIn, Property = "Subject", Values = new List<object> { "a", "test", "in" } }, nameof(StringInAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.StringNotIn, Property = "Subject", Values = new List<object> { "a", "test", "notin" } }, nameof(StringNotInAdvancedFilter) },
            new object[] { new ApiAdvancedFilter{ Type = Data.Enums.FilterType.NumberGreaterThan, Property = "Subject", Values = new List<object> { 5 } }, nameof(NumberGreaterThanAdvancedFilter) }
            
        };

            return allData;
        }
    }
}
