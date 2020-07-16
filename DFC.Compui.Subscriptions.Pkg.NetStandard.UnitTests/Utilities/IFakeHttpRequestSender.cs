using System.Net.Http;

namespace DFC.Compui.Subscriptions.Pkg.Netstandard.UnitTests.Utilities
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}