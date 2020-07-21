using DFC.Compui.Cosmos.Contracts;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IContentItemCacheHandler
    {
        Task ProcessAsync<TModel>(IContentCacheService contentCacheService, TModel contentPageModel) 
            where TModel : IDocumentModel;
    }
}
