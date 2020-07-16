using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Models;
using DFC.Compui.Subscriptions.Pkg.Webhook.Services;
using DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.TestModels;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.UnitTests.WebhookServiceTests
{
    public abstract class BaseWebhooksServiceTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected BaseWebhooksServiceTests()
        {
            Logger = A.Fake<ILogger<WebhooksService<EmailModel>>>();
            FakeEmailEventMessageService = A.Fake<IEventMessageService<EmailModel>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeContentCacheService = A.Fake<IContentCacheService>();
            FakeDocumentService = A.Fake<IDocumentService<EmailModel>>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService<EmailModel>> Logger { get; }

        protected IEventMessageService<EmailModel> FakeEmailEventMessageService { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IContentCacheService FakeContentCacheService { get; }

        protected IDocumentService<EmailModel> FakeDocumentService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }


        protected static EmailApiDataModel BuildValidEmailApiModel()
        {
            var model = new EmailApiDataModel
            {
                Body = "<h1>Test</h1>",
            };

            return model;
        }

        protected static ContactUsApiContentItemModel BuildValidContactUsApiContentItemDataModel()
        {
            var model = new ContactUsApiContentItemModel
            {
                Justify = 1,
                Ordinal = 1,
                Width = 50,
                Content = "<h1>A document</h1>",
            };

            return model;
        }

        protected EmailModel BuildValidEmailModel()
        {
            var model = new EmailModel()
            {
                Id = ContentIdForUpdate,
                Body = "<h1>Test</h1>",
            };

            return model;
        }

        protected IContentItemModel BuildValidContentItemModel(Guid contentItemId)
        {
            var model = new ContentItemModel()
            {
                ItemId = contentItemId,
                Version = Guid.NewGuid(),
                LastReviewed = DateTime.Now,
            };

            return model;
        }

        protected WebhooksService<EmailModel> BuildWebhooksService()
        {
            var service = new WebhooksService<EmailModel>(Logger, FakeMapper, FakeEmailEventMessageService, FakeCmsApiService, FakeDocumentService, FakeContentCacheService);

            return service;
        }
    }
}
