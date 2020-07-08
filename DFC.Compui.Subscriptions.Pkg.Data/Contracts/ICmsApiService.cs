using DFC.Compui.Subscriptions.Pkg.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface ICmsApiService
    {
        Task<IList<TModel>?> GetSummaryAsync<TModel>()
            where TModel : class;

        Task<TModel?> GetItemAsync<TModel>(Uri url)
            where TModel : class;

        Task<TModel?> GetContentItemAsync<TModel>(LinkDetails details)
            where TModel : class;

        Task<TModel?> GetContentItemAsync<TModel>(Uri uri)
            where TModel : class;
    }
}
