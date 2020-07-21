using System;

namespace DFC.Compui.Subscriptions.Pkg.Data.Models
{
    public abstract class ClientOptionsModel
    {
        public Uri? BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 10);         // default to 10 seconds

        public string? ApiKey { get; set; }
    }
}
