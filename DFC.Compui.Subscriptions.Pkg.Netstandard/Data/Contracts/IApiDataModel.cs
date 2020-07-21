using Newtonsoft.Json;
using System;

namespace DFC.Compui.Subscriptions.Pkg.Data.Contracts
{
    public interface IApiDataModel
    {
        [JsonProperty("Uri")]
        Uri? Url { get; set; }
    }
}
