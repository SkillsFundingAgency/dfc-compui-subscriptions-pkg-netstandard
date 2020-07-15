using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IApiService
    {
        Task<string?> GetAsync(HttpClient? httpClient, Uri url, string acceptHeader);

        Task<string?> GetAsync(HttpClient httpClient, string contentType, string acceptHeader);

        Task<HttpStatusCode> PostAsync<TApiModel>(HttpClient? httpClient, Uri url, TApiModel model)
            where TApiModel : class;

        Task<HttpStatusCode> DeleteAsync(HttpClient? httpClient, Uri url);

        Task<string?> GetAsync(HttpClient httpClient, string contentType, string id, string acceptHeader);
    }
}
