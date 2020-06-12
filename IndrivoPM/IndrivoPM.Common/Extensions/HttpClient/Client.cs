namespace Gear.Common.Extensions.HttpClient
{
    public interface IHttpClientAccessor
    {
        System.Net.Http.HttpClient Client { get; }
    }
}
