using AutoMapper;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Services
{
    public class CmsApiService : ICmsApiService
    {
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly IApiDataProcessorService apiDataProcessorService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public CmsApiService(
            CmsApiClientOptions cmsApiClientOptions,
            IApiDataProcessorService apiDataProcessorService,
            HttpClient httpClient,
            IMapper mapper)
        {
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.apiDataProcessorService = apiDataProcessorService;
            this.httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task<IList<TModel>?> GetSummaryAsync<TModel>()
            where TModel : class
        {
            var url = new Uri(
                $"{cmsApiClientOptions.BaseAddress}{cmsApiClientOptions.SummaryEndpoint}",
                UriKind.Absolute);

            return await apiDataProcessorService.GetAsync<IList<TModel>>(httpClient, url)
                .ConfigureAwait(false);
        }

        public async Task<TModel?> GetItemAsync<TModel, TModelChild>(Uri url)
             where TModel : class, IContentItemModel
             where TModelChild : class, IContentItemModel
        {
            var pagesApiDataModel = await apiDataProcessorService.GetAsync<TModel>(httpClient, url)
                .ConfigureAwait(false);

            if (typeof(TModelChild) != typeof(NoChildren))
            {
                if (pagesApiDataModel.ContentItems == null)
                {
                    pagesApiDataModel.ContentItems = new List<IContentItemModel>();
                }

                await GetSharedChildContentItems<TModelChild>(pagesApiDataModel.ContentLinks, pagesApiDataModel.ContentItems).ConfigureAwait(false);
            }

            return pagesApiDataModel;
        }

        public async Task<TModel?> GetContentItemAsync<TModel>(LinkDetails details)
             where TModel : class
        {
            return await apiDataProcessorService.GetAsync<TModel>(httpClient, details.Uri)
                .ConfigureAwait(false);
        }

        public async Task<TModel?> GetContentItemAsync<TModel>(Uri uri)
             where TModel : class
        {
            return await apiDataProcessorService.GetAsync<TModel>(httpClient, uri)
                .ConfigureAwait(false);
        }

        private async Task GetSharedChildContentItems<TModel>(IContentLinks? model, IList<IContentItemModel> contentItem)
            where TModel : class, IContentItemModel
        {
            if (model != null && model.ContentLinks.Any())
            {
                if (contentItem == null)
                {
                    contentItem = new List<IContentItemModel>();
                }

                foreach (var linkDetail in model.ContentLinks.SelectMany(contentLink => contentLink.Value))
                {
                    var pagesApiContentItemModel =
                        await GetContentItemAsync<TModel>(linkDetail).ConfigureAwait(false);

                    if (pagesApiContentItemModel != null)
                    {
                        mapper.Map(linkDetail, pagesApiContentItemModel);
                        await GetSharedChildContentItems<TModel>(pagesApiContentItemModel.ContentLinks, pagesApiContentItemModel.ContentItems!).ConfigureAwait(false);
                        contentItem.Add(pagesApiContentItemModel);
                    }
                }
            }
        }
    }
}