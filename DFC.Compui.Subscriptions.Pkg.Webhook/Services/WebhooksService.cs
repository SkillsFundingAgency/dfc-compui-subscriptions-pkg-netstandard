using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Enums;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Services
{
    public class WebhooksService<TModel> : IWebhooksService
        where TModel : class, IContentItemModel, IDocumentModel
    {
        private readonly ILogger<WebhooksService<TModel>> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService<TModel> eventMessageService;
        private readonly ICmsApiService cmsApiService;
        private readonly IDocumentService<TModel> documentService;
        private readonly IContentCacheService contentCacheService;

        public WebhooksService(
            ILogger<WebhooksService<TModel>> logger,
            AutoMapper.IMapper mapper,
            IEventMessageService<TModel> eventMessageService,
            ICmsApiService cmsApiService,
            IDocumentService<TModel> contentPageService,
            IContentCacheService contentCacheService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.eventMessageService = eventMessageService;
            this.cmsApiService = cmsApiService;
            this.documentService = contentPageService;
            this.contentCacheService = contentCacheService;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, Uri url)
        {
            bool isContentItem = contentCacheService.CheckIsContentItem(contentId);

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    if (isContentItem)
                    {
                        return await DeleteContentItemAsync(contentId).ConfigureAwait(false);
                    }
                    else
                    {
                        return await DeleteContentAsync(contentId).ConfigureAwait(false);
                    }

                case WebhookCacheOperation.CreateOrUpdate:
                    if (isContentItem)
                    {
                        return await ProcessContentItemAsync(url, contentId).ConfigureAwait(false);
                    }
                    else
                    {
                        return await ProcessContentAsync(url, contentId).ConfigureAwait(false);
                    }

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> ProcessContentAsync(Uri url, Guid contentId)
        {
            var apiDataModel = await cmsApiService.GetItemAsync<TModel>(url).ConfigureAwait(false);
            var contentModel = mapper.Map<TModel>(apiDataModel);

            if (contentModel == null)
            {
                return HttpStatusCode.NoContent;
            }

            if (!TryValidateModel(contentModel))
            {
                return HttpStatusCode.BadRequest;
            }

            var contentResult = await eventMessageService.UpdateAsync(contentModel).ConfigureAwait(false);

            if (contentResult == HttpStatusCode.NotFound)
            {
                contentResult = await eventMessageService.CreateAsync(contentModel).ConfigureAwait(false);
            }

            if (contentResult == HttpStatusCode.OK || contentResult == HttpStatusCode.Created)
            {
                if (contentModel.ContentItems != null)
                {
                    var contentItemIds = (from a in contentModel.ContentItems select a.ItemId!.Value).ToList();

                    contentCacheService.AddOrReplace(contentId, contentItemIds);
                }
            }

            return contentResult;
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            var result = await eventMessageService.DeleteAsync(contentId).ConfigureAwait(false);

            if (result == HttpStatusCode.OK)
            {
                contentCacheService.Remove(contentId);
            }

            return result;
        }

        public async Task<HttpStatusCode> DeleteContentItemAsync(Guid contentItemId)
        {
            var contentIds = contentCacheService.GetContentIdsContainingContentItemId(contentItemId);

            if (!contentIds.Any())
            {
                return HttpStatusCode.NoContent;
            }

            foreach (var contentId in contentIds)
            {
                var contentPageModel = await documentService.GetByIdAsync(contentId).ConfigureAwait(false);

                if (contentPageModel != null)
                {
                    var contentItemModel = contentPageModel.ContentItems?.FirstOrDefault(f => f.ItemId == contentItemId);

                    if (contentItemModel != null)
                    {
                        contentPageModel.ContentItems!.Remove(contentItemModel);

                        var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

                        if (result == HttpStatusCode.OK)
                        {
                            contentCacheService.RemoveContentItem(contentId, contentItemId);
                        }
                    }
                }
            }

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> ProcessContentItemAsync(Uri url, Guid contentItemId)
        {
            var contentIds = contentCacheService.GetContentIdsContainingContentItemId(contentItemId);

            if (!contentIds.Any())
            {
                return HttpStatusCode.NoContent;
            }

            var apiDataContentItemModel = await cmsApiService.GetContentItemAsync<TModel>(url).ConfigureAwait(false);

            foreach (var contentId in contentIds)
            {
                var contentPageModel = await documentService.GetByIdAsync(contentId).ConfigureAwait(false);

                if (contentPageModel != null)
                {
                    var contentItemModel = contentPageModel.ContentItems?.FirstOrDefault(f => f.ItemId == contentItemId);

                    if (contentItemModel != null)
                    {
                        mapper.Map(apiDataContentItemModel, contentItemModel);

                        await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);
                    }
                }
            }

            return HttpStatusCode.OK;
        }

        public bool TryValidateModel(TModel contentModel)
        {
            _ = contentModel ?? throw new ArgumentNullException(nameof(contentModel));

            var validationContext = new ValidationContext(contentModel, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(contentModel, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {nameof(TModel)}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
