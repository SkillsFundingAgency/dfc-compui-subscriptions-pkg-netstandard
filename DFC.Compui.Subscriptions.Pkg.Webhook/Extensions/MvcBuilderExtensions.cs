using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddWebhookController(this IMvcBuilder builder)
        {
            builder.AddApplicationPart(Assembly.Load("DFC.Compui.Subscriptions.Pkg.Webhook"));
            return builder;
        }
    }
}
