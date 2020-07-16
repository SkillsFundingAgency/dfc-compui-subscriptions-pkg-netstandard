﻿using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Services
{
    public class EventMessageService<TModel> : IEventMessageService<TModel>
           where TModel : class, IDocumentModel
    {
        private readonly ILogger<EventMessageService<TModel>> logger;
        private readonly IDocumentService<TModel> contentPageService;

        public EventMessageService(ILogger<EventMessageService<TModel>> logger, IDocumentService<TModel> contentPageService)
        {
            this.logger = logger;
            this.contentPageService = contentPageService;
        }

        public async Task<IList<TModel>?> GetAllCachedCanonicalNamesAsync()
        {
            var serviceDataModels = await contentPageService.GetAllAsync().ConfigureAwait(false);

            return serviceDataModels.ToList();
        }

        public async Task<HttpStatusCode> CreateAsync(TModel? upsertDocumentModel)
        {
            if (upsertDocumentModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertDocumentModel.Id).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return HttpStatusCode.AlreadyReported;
            }

            var response = await contentPageService.UpsertAsync(upsertDocumentModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(CreateAsync)} has upserted content for: {upsertDocumentModel.Id} with response code {response}");

            return response;
        }

        public async Task<HttpStatusCode> UpdateAsync(TModel? upsertDocumentModel)
        {
            if (upsertDocumentModel == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var existingDocument = await contentPageService.GetByIdAsync(upsertDocumentModel.Id).ConfigureAwait(false);
            if (existingDocument == null)
            {
                return HttpStatusCode.NotFound;
            }

            upsertDocumentModel.Etag = existingDocument.Etag;

            var response = await contentPageService.UpsertAsync(upsertDocumentModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(UpdateAsync)} has upserted content for: {upsertDocumentModel.Id} with response code {response}");

            return response;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var isDeleted = await contentPageService.DeleteAsync(id).ConfigureAwait(false);

            if (isDeleted)
            {
                logger.LogInformation($"{nameof(DeleteAsync)} has deleted content for document Id: {id}");
                return HttpStatusCode.OK;
            }
            else
            {
                logger.LogWarning($"{nameof(DeleteAsync)} has returned no content for: {id}");
                return HttpStatusCode.NotFound;
            }
        }
    }
}
