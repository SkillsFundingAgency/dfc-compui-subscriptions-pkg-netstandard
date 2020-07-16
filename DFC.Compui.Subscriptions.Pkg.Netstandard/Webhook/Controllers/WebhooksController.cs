using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Controllers
{
    [Route("api/webhook")]
    public class WebhooksController : Controller
    {
        private readonly ILogger<WebhooksController> logger;
        private readonly IWebhookReceiver webhookReceiver;

        public WebhooksController(
            ILogger<WebhooksController> logger,
            IWebhookReceiver webhookReceiver)
        {
            this.logger = logger;
            this.webhookReceiver = webhookReceiver;
        }

        [HttpPost]
        [Route("ReceiveEvents")]
        public async Task<IActionResult> ReceiveEvents()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string requestContent = await reader.ReadToEndAsync().ConfigureAwait(false);
            logger.LogInformation($"Received events: {requestContent}");

            var result = await webhookReceiver.ReceiveEvents(requestContent);

            return result;
        }
    }
}