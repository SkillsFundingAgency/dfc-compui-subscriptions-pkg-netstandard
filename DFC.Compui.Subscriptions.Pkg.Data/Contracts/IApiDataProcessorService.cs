using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IApiDataProcessorService
    {
        Task<TApiModel?> GetAsync<TApiModel>(HttpClient? httpClient, Uri url)
            where TApiModel : class;

        Task<HttpStatusCode> PostAsync<TApiModel>(HttpClient? httpClient, Uri url, TApiModel model)
            where TApiModel : class;

        Task<HttpStatusCode> DeleteAsync(HttpClient? httpClient, Uri url);

        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType)
            where TApiModel : class;

        Task<TApiModel?> GetAsync<TApiModel>(HttpClient httpClient, string contentType, string id)
            where TApiModel : class;
    }
}
