using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IWebhookReceiver
    {
        Task<IActionResult> ReceiveEvents(string eventContent);
    }
}
